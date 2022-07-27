from api import ConnectApi
from manager import Hour



params_hour = Hour.get_execute_time()
print('Giskard: la hora', params_hour)



end_point = "https://msd-fra-mtf-scorefraudes-dev-dinersclub-migracion-dev.apps.din-ros-can-dev.9gqx.p1.openshiftapps.com/fraude/v1/marcas/consultar"


params_query_score = {
    "fechaConsulta": "2022-07-01",
    "horaInicio": "12.12",
    "horaFin": "13.13"
}
results = ConnectApi.connect(end_point, params_query_score)
print('Giskard: ',  results)
