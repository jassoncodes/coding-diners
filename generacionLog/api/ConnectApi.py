import requests
from requests.exceptions import HTTPError
import uuid

#Should connect with microservice send body       
def connect():
    return {}

#Should return a structure of data  used in microservice
def get_body():
    uuid_app = get_uuid()
    body_data = {
        "dinHeader": {
            "aplicacionId": "RPA",
            "canalId": "RPA",
            "uuid": uuid_app
        }
    }
    return body_data
#Should get a uuid code
def get_uuid():
    my_uuid = uuid.uuid4()
    return str(my_uuid)