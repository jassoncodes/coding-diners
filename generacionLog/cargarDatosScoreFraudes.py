from api.ConnectApi import connect_api
from hooks.Hour import Hour 
from reportes.execution_report import execution_report

config = "F:\\AsistenteLogScoreFraude\\config\\config_reportes.json"
minute_load_data = 30
end_point = "http://10.10.176.150:8299/ms-gestion-financiera-riesgos/dev/marcas/consultar"


hour_utilities = Hour(config)
hour_utilities.get_query_params(minute_load_data)
query_score = hour_utilities.results
date_report = hour_utilities.date_report

cnx_microservice = connect_api(config, query_score)
cnx_microservice.connect(end_point)
query_results = cnx_microservice.results

report_execution = execution_report(config)
if query_results:
    print('RPA-GenarciónLogScore: ', 'seguir')
    report_execution.proccess_data(query_results, query_score, date_report)
    report_execution_config = report_execution.results
    report_execution.create_report(report_execution_config)
else:
    print('RPA-GenarciónLogScore: ', 'microservicio no disponible')
    report_execution.chance_status_all("desactivate")

