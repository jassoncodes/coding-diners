
from api import ConnectApi
from manager import Hour, Report, Email


hour = Hour.get_execute_time()
date = Hour.get_date_calc()

minute_load_data = 30
end_point = "http://10.10.176.150:8299/ms-gestion-financiera-riesgos/dev/marcas/consultar"
config = "E:\\AsistenteLogScoreFraude\\config\\config_reportes.json"

ruta_final= Report.copy_template(config)
query_score = Hour.get_diference_hour(minute_load_data, hour.hour_init, date.date_calc)
date_report = Hour.get_date_report()


params_query_score = {
    "fechaConsulta": query_score.date_search,
    "horaInicio": query_score.hour_init,
    "horaFin": query_score.hour_end
}
results = ConnectApi.connect(end_point, params_query_score)
print('Giskard: ', results)


if "datos" in results["dinBody"]:
    execution_records = results["dinBody"]["datos"]
    for execution_record in execution_records:
        
        if int(execution_record["registros"]) == 0:
            observation = "Se recomienda hacer un proceso Manual"
            print('Giskard: enviar la notificacion', execution_record)
            email_sender = {
                "subject": "Notificación de Proceso Manual",
                "content": "Se recomienda realizar un proceso manual para la marca: "+execution_record["marca"]+" "
            }
            Email.sender_email(config, email_sender)
            Report.chance_status(execution_record["marca"], config, "desactivate")
            
        else:
            observation = results["dinError"]["mensaje"]
            
        execution_record["fecha_ejecucion"] = str(query_score.date_search)
        execution_record["hour_init"] = query_score.hour_init
        execution_record["hour_end"] = query_score.hour_end
        execution_record["hour_execute"] = query_score.hour_execute
        execution_record["year"] = date_report.year
        execution_record["month"] = date_report.month_name
        execution_record["observations"] = observation

        
        
params_report = {
    "path": ruta_final,
    "data": execution_records,
    "sheet_name": "report",
    "cell_init_write": "A"
}


Report.create_report(params_report)



