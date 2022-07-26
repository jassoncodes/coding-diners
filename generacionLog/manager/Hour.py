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
    
    return results

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