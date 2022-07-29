from manager  import Hour
from unittest.mock import patch
from unittest import mock
import datetime
from freezegun import freeze_time

#Should to obtain params used ein query micorservice
@freeze_time()
@patch("manager.Hour.datetime.now")
def test_get_diference_hour(mock_datetime_now):
    minute_load_data = 30
    hour_init = "12.00"
    date_calc =  datetime.datetime(2022, 7, 22, 12, 0, 0)
    
    mock_datetime_now.return_value = datetime.datetime(2022, 7, 22, 12, 0, 0)
    mock_datetime_now.strftime.return_value = datetime.datetime(2022, 7, 22, 12, 0, 0)
    
    
    results_hour = Hour.get_diference_hour(
        minute_load_data, 
        hour_init, 
        date_calc
    )
    expected = {
            "hour_init": "11.30",
            "hour_end": "12.00",
            "date_search": "2022-07-22",
            "hour_execute": "12:00:00"
    }
    assert expected == results_hour
    mock_datetime_now.assert_called_once()
    
#Should to obtanin date now  system
@freeze_time()
@patch("manager.Hour.datetime.now")
def test_get_hour(mock_datetime_now):
    mock_datetime_now.return_value = datetime.datetime(2022, 7, 22, 12, 0, 0)
    results_hour = Hour.get_hour()
    expected = "12:00:00"
    assert expected == results_hour
    mock_datetime_now.assert_called_once()
    
#Should  to obtain hour params to find in used microservice
def test_get_time_search():
    date_calc = datetime.datetime(2022, 7, 22, 12, 0, 0)
    minute_load_data = 30
    now_time = ["12", "00"]
    results_time = Hour.get_time_search(date_calc, minute_load_data, now_time)
    expected_results = {
        "day_minor": "2022-07-22",
        "hour_minor": "11:30:00"
    }
    assert results_time == expected_results
    
#Should return time and date
@freeze_time()
@patch("manager.Hour.datetime.now")
def test_get_execute_time(mock_now):
    mock_now.return_value = datetime.datetime(2022,7,1, 17,4,0)
    expected = {
        "hour_init": "17.00"
    }
    results = Hour.get_execute_time()
    assert expected == results
    mock_now.assert_called_once()
    

#Should return date_calc
@freeze_time()
@patch("manager.Hour.datetime.now")
def test_date_calc(mock_now):
    mock_date_calc = datetime.datetime(2022, 7, 27)
    mock_now.return_value = mock_date_calc
    expected = {"date_calc": mock_date_calc}
    results = Hour.get_date_calc()
    assert expected == results
    mock_now.assert_called_once()
    