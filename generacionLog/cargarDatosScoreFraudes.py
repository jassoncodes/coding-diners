from api.ConnectApi import connect_api
from hooks.Hour import Hour 
from hooks.Report import Report

config = "F:\\AsistenteLogScoreFraude\\config\\config_reportes.json"
minute_load_data = 30
end_point = "http://10.10.176.150:8299/ms-gestion-financiera-riesgos/dev/marcas/consultar"


report_write = Report(config)

hour_utilities = Hour(config)
hour_utilities.get_query_params(minute_load_data)
query_score = hour_utilities.results
date_report = hour_utilities.date_report




cnx_microservice = connect_api(config, query_score)
cnx_microservice.connect(end_point)
query_reults = cnx_microservice.results

if "datos" in query_reults["dinBody"]:
    execution_records = query_reults["dinBody"]["datos"]
    for execution_record in execution_records:
        
        if int(execution_record["registros"]) == 0:
            observation = "Se recomienda hacer un proceso Manual"
            # print('Giskard: enviar la notificacion', execution_record)
            email_sender = {
                "subject": "Notificación de Proceso Manual",
                "content": "Se recomienda realizar un proceso manual para la marca: "+execution_record["marca"]+" "
            }
            # Email.sender_email(config, email_sender)
            report_write.chance_status(execution_record["marca"], "desactivate")
            
        else:
            observation = query_reults["dinError"]["mensaje"]
            
        execution_record["fecha_ejecucion"] = str(query_score.date_search)
        execution_record["hour_init"] = query_score.hour_init
        execution_record["hour_end"] = query_score.hour_end
        execution_record["hour_execute"] = query_score.hour_execute
        execution_record["year"] = date_report.year
        execution_record["month"] = date_report.month_name
        execution_record["observations"] = observation


    params_report = {
        "path": report_write.ruta_final,
        "data": execution_records,
        "sheet_name": "report",
        "cell_init_write": "A"
    }
    
    report_write.create_report(params_report)