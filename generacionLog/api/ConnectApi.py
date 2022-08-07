import HoockUtilities as helper
from requests.exceptions import HTTPError
import requests
import socket
import uuid
import time
import sys

log_bot = "F:\\AsistenteLogScoreFraude\\"
#Should connect with microservice send body       
def connect(end_point, params_query_score):
    body_query = get_body(params_query_score)
    try:
        response = requests.post(end_point, json=body_query)
        if int(response.status_code) == 200:
            return response.json()
        else:
            return {}
    except IOError as error:
        except_info = sys.exc_info()
        s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
        helper.put_log(s_message,"--","ConnectApi", log_bot+"/connectApi.txt")

#Should return a structure of data  used in microservice
def get_body(params_query):
    uuid_app = get_uuid()
    ip_host = get_ip()
    date_query = get_date()
    body_data = {
        "dinHeader": {
            "aplicacionId": "RPA",
            "canalId": "RPA",
            "uuid": uuid_app,
            "ip": ip_host,
            "horaTransaccion": date_query
        },
        "dinBody": params_query
    }
    return body_data
#Should get a uuid code
def get_uuid():
    my_uuid = uuid.uuid4()
    return str(my_uuid)

#Should ip del euipo
def get_ip():
    host_name = socket.gethostname()
    ip = socket.gethostbyname(host_name)
    return ip

#Should get la fecha de creacion
def get_date():
    now = time.localtime()
    T_stamp = time.strftime("%Y-%m-%d %H:%M:%S", now) 
    return str(T_stamp)