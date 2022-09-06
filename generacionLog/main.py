
from api import ConnectApi
from helpers import Hour, Report, Email
import sys
import os

hour = Hour.get_execute_time()
date = Hour.get_date_calc()

minute_load_data = 30
end_point = "http://10.10.176.150:8299/ms-gestion-financiera-riesgos/dev/marcas/consultar"
config = "F:\\AsistenteLogScoreFraude\\config\\config_reportes.json"

try:
    log_bot = "F:\\AsistenteLogScoreFraude\\"
    # if(len(sys.argv)>1):
    if(True):
        
        
        # minute_load_data = int(sys.argv[1])
        # end_point = str(sys.argv[2])
        # config = str(sys.argv[3])
        

        ruta_final = Report.copy_template(config)
        query_score = Hour.get_diference_hour(minute_load_data, hour.hour_init, date.date_calc)
        date_report = Hour.get_date_report()


        params_query_score = {
            "fechaConsulta": query_score.date_search,
            "horaInicio": query_score.hour_init,
            "horaFin": query_score.hour_end
        }
        
        print('Giskard: ', params_query_score)
        results = ConnectApi.connect(end_point, params_query_score)
        # print('Giskard: ', results)
        #Revisar  el proceso de los casos 400 y 500

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
                    observation = "Sin Registros"
                else:
                    # observation = results["dinError"]["mensaje"]
                     observation = "Satisfactorio"
                     
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
    else:
        s_message = "No ingreso los parametros de manera correcta"
        
        Report.helpers.put_log(s_message,"--","main", str(log_bot)+"/main.txt")
        

except IOError as error:
    except_info = sys.exc_info()
    s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
    Report.helpers.put_log(s_message,"--","main",  str(log_bot)+"/main.txt")
