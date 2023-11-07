from modules.functions import *
from dotenv import load_dotenv
import os
import pandas as pd
from flask import Flask, jsonify, request
import mysql.connector


app = Flask(__name__)
# Cargar las variables de entorno
load_dotenv()
db_host = os.getenv("DB_HOST")
db_port = os.getenv("DB_PORT")
db_user = os.getenv("DB_USER")
db_password = os.getenv("DB_PASSWORD")
db_name = os.getenv("DB_NAME")


# Crear una conexión SQLAlchemy
db = mysql.connector.connect(
    host=db_host,
    port=int(db_port),
    user=db_user,
    password=db_password,
    database=db_name
)

#funcion para validar token
def validate_token(token):
    """Funcion que recibe un token y retorna un booleano que indica si el token es valido o no"""
    key = os.getenv("KEY")
    if key == None:
        return jsonify ({'message': 'Es necesario una clave de seguridad para inciar la api'}),406
    elif key == token:
        return True
    else:
        return False
    


#middleware para verificar la autenticacion
@app.before_request
def check_authentication():
    """"Funcion que verifica la autenticacion antes de ejecutar una peticion, obteniendo el token de autenticacion mediante el encabezado"""
    if request.endpoint == 'get_result':
        token = request.headers.get('Authorization')
        if not validate_token(token):
            return jsonify({'message': 'Autenticación fallida'}), 401


@app.route('/predictions', methods=['POST'])
def get_result():
    """  Función que recibe un json con una lista de ids y retorna un json con la información de cada id y su predicción"""
    try:
        request_data = request.get_json()
        id_list = request_data.get('ids', [])
        predictions = []

        for i, id_obj in enumerate(id_list, 1):
            dpi = id_obj.get('dpi', None)
            if dpi is None:
                return jsonify({'message': 'Invalid Id object'}), 400

            sql_information = get_information(dpi)
            information_client = pd.read_sql(sql_information, con=db)

            sql_prediction = get_prediction_information(dpi)
            information = pd.read_sql(sql_prediction, con=db)

            if information.empty:
                return jsonify({'message': 'ID not found'}), 404

            predict = predict_data(information)
            information_client = process_info(predict, information_client)
            predictions.append({f'Information_{i}': information_client})

        return jsonify({'Resultado': predictions})

    except Exception as e:
        return jsonify({'error': str(e)}), 500
