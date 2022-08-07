
from ntpath import join
import HoockUtilities as helpers
from datetime import datetime
#Should to obtain params necesary that used in query microservice
def get_diference_hour(minute_load_data, hour_init, date_calc):
    results = {}
    hour_now = get_hour()
    now_time = hour_init.strip().split(".")

    if hour_init.strip().split(".")[0] == "00"  and int(hour_init.strip().split(".")[1]) < 30:
        now_time[0] = 00
        now_time[1] = 00
        
    time_search = get_time_search(date_calc, minute_load_data, now_time)
    hour_ = time_search["hour_minor"].replace(":", ".").split(".")
    
    if hour_init.strip().split(".")[0] == "00" and int(hour_init.strip().split(".")[1]) < 30:
        hour_init="24.00"
        time_search["day_minor"] = date_calc.strftime("%Y-%m-%d")
    
    
    results = {
        "hour_init": hour_[0]+"."+hour_[1],
        "hour_end": hour_init.strip(),
        "date_search": str(time_search["day_minor"]),
        "hour_execute": str(hour_now)
    }
    
    return helpers.dictToObject(results)

#Should rto response with hour of system
def get_hour():
    now_system = datetime.now()
    hour_now = now_system.strftime("%H:%M:%S")
    return hour_now

#Should  to obtain hour params to find in used microservice
def get_time_search(date_calc, minute_load_data, now_time):
    year = int(date_calc.year)
    mouth = int(date_calc.month)
    day = int(date_calc.day)

    date_complet_now = datetime(year, mouth, day, int(now_time[0]), int(now_time[1]), 00)
    time_search = helpers.rest_minute(date_complet_now, minute_load_data)
    return time_search

#Should return time and date
def get_execute_time():
    time_search = datetime.now()
    #Delay de disponibilidad de información en la base de datos de Riesgos
    minutes = 4
    time_init = helpers.rest_minute(time_search, minutes)
    hour_time = time_init["hour_minor"].replace(":", ".").split(".")
    date_execute_time = {
        "hour_init":hour_time[0]+"."+hour_time[1]
    }
    return helpers.dictToObject(date_execute_time)
    
#Should return date_calc
def get_date_calc():
    date_calc = {
        "date_calc": datetime.now()
    }
    return helpers.dictToObject(date_calc)


def get_name_mouth(number_mouth):
    mouth = [
        '', "ENERO", "FEBRERO", "MARZO",
        'ABRIL', "MAYO", "JUNIO", "JULIO",
        'AGOSTO', "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE",
        'DICIEMBRE'
    ]
    return mouth[int(number_mouth)]
    
def get_date_report():
    now_system = datetime.now()
    year = now_system.year
    mouth = get_name_mouth(now_system.month)
    date_report = {
        "year": year,
        "month_name": mouth
    }
    return helpers.dictToObject(date_report)