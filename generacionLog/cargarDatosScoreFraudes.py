import sys
from api.ConnectApi import connect_api
from hooks.Hour import Hour 
from reportes.execution_report import execution_report
from hooks.Email import Email

# config = "E:\\AsistenteLogScoreFraude\\config\\config_reportes.json"
# minute_load_data = 30
# end_point = "http://10.10.176.150:8299/ms-gestion-financiera-riesgos/dev/marcas/consultar"


try:
    if(len(sys.argv)>1):
        config = sys.argv[1]
        end_point = sys.argv[2]
        minute_load_data = int(sys.argv[3])
        print('Giskard: parametros de entrada ', config, end_point, str(minute_load_data), type(minute_load_data))

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
            subject = "Notificación de Proceso Manual"
            content ="Se recomienda realizar un proceso manual, microservicio no está disponible "
            Email(config).sender_email(subject, content)    
            report_execution.chance_status_all("desactivate")
        
except IOError as error:
    except_info = sys.exc_info()
    s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
    hour_utilities.put_log(s_message,"--","CargarDatosScoreFraudes", hour_utilities.log+"/CargarDatosScoreFraudes.txt")
