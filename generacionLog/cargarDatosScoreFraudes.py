from api.ConnectApi import ConnectApi
from hooks.Hour import Hour
from hooks.Report import  Report
from hooks.Email import Email

import sys


try:
    if(len(sys.argv)>1):
        config = str(sys.argv[1])
        end_point = str(sys.argv[2])
        minute_load_data = int(sys.argv[3])
        

        
        date_object = Hour(config)
        report_object = Report(config)
        email_object = Email(config)
        date = date_object.get_date_calc()
        hour = date_object.get_execute_time()
        
        

        ruta_final = report_object.copy_template(config)
        query_score = date_object.get_diference_hour(minute_load_data, hour.hour_init, date.date_calc)
        date_report = date_object.get_date_report()


        params_query_score = {
            "date_search": query_score.date_search,
            "hour_init": query_score.hour_init,
            "hour_end": query_score.hour_end
        }
        
        #Pruebas
        params_query_score = {
            "date_search": "2022-08-24",
            "hour_init": "00.00",
            "hour_end": "01.00"
        }
        

        conecct_object = ConnectApi(config, params_query_score)
        results = conecct_object.connect(end_point)
        transaction_data = conecct_object.results
        isNullTransaction = transaction_data["dinBody"] is None
        
        if not isNullTransaction and  "datos" in transaction_data["dinBody"]:
            execution_records = transaction_data["dinBody"]["datos"]
            for execution_record in execution_records:
                
                if int(execution_record["registros"]) == 0:
                    observation = "Se recomienda hacer un proceso Manual"
                    email_sender = {
                        "subject": "Notificación de Proceso Manual",
                        "content": "Se recomienda realizar un proceso manual para la marca: "+execution_record["marca"]+" "
                    }
                    email_object.sender_email(config, email_sender)
                    report_object.chance_status(execution_record["marca"], "desactivate")
                    observation = "Sin Registros"
                else:
                    
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


            report_object.create_report(params_report)
            
    
        else:
            email_sender = {
                "subject": "Notificación de Proceso Manual",
                "content": "Se recomienda realizar un proceso manual, los parámetros de la consulta no son los correctos "
            }
            email_object.sender_email(config, email_sender)

    
    
        

except IOError as error:
    except_info = sys.exc_info()
    s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
    report_object.helpers.put_log(s_message,"--","main",  str(config)+"/main.txt")
