from munch import DefaultMunch
from api import ConnectApi
from manager import Hour


hour = Hour.get_execute_time()
date = Hour.get_date_calc()

minute_load_data = 30
end_point = "https://msd-fra-mtf-scorefraudes-dev-dinersclub-migracion-dev.apps.din-ros-can-dev.9gqx.p1.openshiftapps.com/fraude/v1/marcas/consultar"

query_score = Hour.get_diference_hour(minute_load_data, hour.hour_init, date.date_calc)



params_query_score = {
    "fechaConsulta": query_score.date_search,
    "horaInicio": query_score.hour_init,
    "horaFin": query_score.hour_end
}
results = ConnectApi.connect(end_point, params_query_score)



if "datos" in results["dinBody"]:
    execution_records = results["dinBody"]["datos"]
    for execution_record in execution_records:
        execution_record["fecha_ejecucion"] = query_score.date_search
        execution_record["hour_init"] = query_score.hour_init
        execution_record["hour_end"] = query_score.hour_end
        execution_record["hour_execute"] = query_score.hour_execute
        
        print('Giskard: ', execution_record)
