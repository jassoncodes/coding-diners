import requests
import socket
from requests.exceptions import HTTPError
import uuid

#Should connect with microservice send body       
def connect():
    return {}

#Should return a structure of data  used in microservice
def get_body():
    uuid_app = get_uuid()
    ip_host = get_ip()
    body_data = {
        "dinHeader": {
            "aplicacionId": "RPA",
            "canalId": "RPA",
            "uuid": uuid_app,
            "ip": ip_host
        }
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