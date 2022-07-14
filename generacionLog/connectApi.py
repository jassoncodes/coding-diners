import requests
from requests.exceptions import HTTPError
import uuid
end_point = "http://10.10.176.150:8299/ms-productos/desarrollo/marcas/consultar"
myuuid = uuid.uuid4()
body_data =  { 
  "dinHeader": {
    "aplicacionId": "RPA",
    "canalId": "RPA",
    "uuid": str(myuuid),
    "ip": "10.100.68.168",
    "horaTransaccion": "2022-07-13T09:23:06.486"
  },
  "dinBody": {
    "fechaConsulta": "2022-07-01",
    "horaInicio": "12.12",
    "horaFin": "13.13"
  }
}
try:
    response = requests.post(end_point, json=body_data)
    if response.status_code == 200:
        json_results = response.json()
        list_mark = json_results["dinBody"]
        if "dinBody" in json_results and "datos" in list_mark:
            for mark in list_mark["datos"]:
                mark_consult = mark["marca"]
                register_consult = mark["registros"]
                if int(register_consult):
                    print('TaskBot: ', mark["marca"], mark["registros"])
                else:
                    print('TaskBot: realizar el proceso de manera manual para la marca: ', mark_consult)
except HTTPError as error:
    print('TaskBot: ', error)
    pass