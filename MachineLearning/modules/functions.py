import pandas as pd
import json
from tensorflow import keras
import pandas as pd
import numpy as np
import os

os.environ['CUDA_VISIBLE_DEVICES'] = '-1'


#crear una funcion que determine el modelo segun su sistema operativo
def get_model():
    """Funcion que determina el modelo segun el sistema operativo"""
    if os.name == 'nt':
        model = 'model.keras'
        weights = 'weights.keras'
        model = keras.models.load_model(model)
        model.load_weights(weights)
        return model
    else:
        model = 'model.h5'
        weights = 'weights.h5'
        model = keras.models.load_model(model)
        model.load_weights(weights)
        return model
    

model = get_model()



#Prescicion del modelo
Precision = '0.995'
error = '0.015'

def get_information(id):
    """ funcion que recibe un id y retorna una querie con la informacion del cliente"""
    querie = f"""
               SELECT * FROM (
             SELECT t1.nombreCompleto, t1.fechaNacimiento, t1.dpi, t1.departamento, t1.municipio, t1.genero
             FROM adm_cliente t1
            INNER JOIN adm_deuda t2
             ON t1.clienteId = t2.clienteId
             INNER JOIN adm_bitacora t3
             ON t3.deudaId = t2.deudaId
             WHERE  t1.dpi = {id}
             UNION
             SELECT t1.nombreCompleto, t1.fechaNacimiento, t1.dpi, t1.departamento, t1.municipio, t1.genero FROM adm_cliente t1
             INNER JOIN adm_deuda t2
             ON t1.clienteId = t2.clienteId
             INNER JOIN adm_bitacora_pago t3
             ON t3.deudaId = t2.deudaId
             WHERE  t1.dpi = {id}
            )
             AS t1
            """
    return querie

def get_prediction_information(id):
    """ funcion que recibe un id y retorna una querie con la informacion de la ultima deuda del cliente"""
    querie = f"""SELECT tipologiaPrimariaId, tipologiaSecundariaId, pesoPrioridad
                    FROM (
                    SELECT t3.tipologiaPrimariaId, t3.tipologiaSecundariaId, t3.pesoPrioridad, t3.fechaCreacion
                    FROM adm_cliente t1
                    INNER JOIN adm_deuda t2 ON t1.clienteId = t2.clienteId
                    INNER JOIN adm_bitacora t3 ON t3.deudaId = t2.deudaId
                    WHERE t1.dpi = {id}
                    UNION
                    SELECT t3.tipologiaPrimariaId, t3.tipologiaSecundariaId, t3.pesoPrioridad, t3.fechaCreacion
                    FROM adm_cliente t1
                    INNER JOIN adm_deuda t2 ON t1.clienteId = t2.clienteId
                    INNER JOIN adm_bitacora t3 ON t3.deudaId = t2.deudaId
                    WHERE t1.dpi = {id}
                    ORDER BY fechaCreacion DESC
                    LIMIT 1
                    ) AS Subquery;"""
    return querie


def predict(a):
    """funcion que recibe un dataframe y retorna la prediccion del modelo"""
    if a.empty:
        return 0.0
    else:
        predict = model.predict(a)
        predict = predict[0]
        return predict

def predict_data(a):
    """ funcion que recibe un dataframe y retorna la prediccion del modelo"""
    predict = model.predict(a)
    info_as_percentage = predict[0] * 100
    info_as_percentage = round(info_as_percentage[0])
    if info_as_percentage >= 100:
        info_as_percentage = 'El cliente pagara su deuda'
        return info_as_percentage
    else:
        info_as_percentage = 'El cliente no pagara su deuda'
        return info_as_percentage

def process_info(a, b):
    """ funcion que recibe un dataframe y una lista de diccionarios y retorna una lista de diccionarios con la informacion del dataframe y la prediccion del modelo"""
    info = a
    b['prediccion'] = info
    b['precision'] = Precision
    b.rename(columns ={'dpi': 'DPI', 'nombreCompleto':'Nombre Completo'}, inplace=True)
    information_client = b.to_dict(orient='records')
    return information_client

