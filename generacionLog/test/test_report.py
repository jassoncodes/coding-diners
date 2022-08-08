import datetime
from unittest.mock import patch
from unittest import mock
from freezegun import freeze_time

from manager import Report

#Should read config from json
@patch("manager.Report.helpers.read_json")
def test_read_config(mock_read_json):
    mock_read_json.return_value = {
        "config": [
            {"principal": ""},
            {"ruta_reporte": ""}, 
        ]
    }
    config_path = ""
    results = Report.read_config(config_path)
    expected = {
        "config": [
            {"principal": ""},
            {"ruta_reporte": ""}, 
        ]
    }
    assert results == expected
    mock_read_json.assert_called_with(config_path)


#Should copy File path principal to move other path
# @patch("manager.Report.helpers.dictToObject")
@patch("manager.Report.helpers.copy_file")
@patch("manager.Report.helpers.get_date_complete")
@patch("manager.Report.read_config")
def test_copy_template(mock_read_config, mock_get_date_complete, mock_copy_file):
    mock_read_config.return_value = {
        "principal": "F:\\AsistenteLogScoreFraude\\config\\",
        "ruta_reporte": "F:\\Reporte_bot\\",
    }
    date_mock = "2022-07-29" 
    mock_get_date_complete.return_value = date_mock
    config = "F:\\AsistenteLogScoreFraude\\config\\"
    results = Report.copy_template(config)
    expected = "F:\\Reporte_bot\\reporte_ejecucion_"+date_mock+".xlsx"
    
    assert results == expected
    mock_read_config.assert_called_with(config)
    mock_get_date_complete.assert_called_once()
    mock_copy_file.assert_called_once()
    
