from api import ConnectApi 

end_point = "http://10.10.176.150:8299/ms-gestion-financiera-riesgos/dev/marcas/consultar"
params_query_score = {
    "fechaConsulta": "2022-07-01",
    "horaInicio": "12.12",
    "horaFin": "13.13"
}
results = ConnectApi.connect(end_point, params_query_score)