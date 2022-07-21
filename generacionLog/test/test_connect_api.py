from unittest.mock import patch
from api import ConnectApi

#Should connect with microservice 
def test_connect_api():
    params_mock = {}
    results = ConnectApi.connect()
    assert results == {}
    
#Should create body request used in microservice
@patch("api.ConnectApi.get_ip")
@patch("api.ConnectApi.get_uuid")
def test_get_body_request(mock_get_uuid, mock_get_ip):
    uuid_mock = "773684d0-02b6-11ed-b939-0242ac120003"
    mock_ip ="10.100.68.168"
    mock_get_uuid.return_value = uuid_mock
    mock_get_ip.return_value = mock_ip

    body_mock = { 
        "dinHeader": {
            "aplicacionId": "RPA",
            "canalId": "RPA",
            "uuid": uuid_mock,
            "ip": mock_ip
        }
    }
    expected_results = ConnectApi.get_body()
    assert  body_mock == expected_results
    mock_get_uuid.assert_called_once()
    mock_get_ip.assert_called_once()
    
    
#Should generate uuid automatic
@patch("api.ConnectApi.uuid.uuid4")
def test_get_uuid(mock_uuid4):
    uuid_mock = "773684d0-02b6-11ed-b939-0242ac120003"
    mock_uuid4.return_value = uuid_mock
    expected = ConnectApi.get_uuid()
    assert expected == uuid_mock
    mock_uuid4.assert_called_once()
    

#Should get ip del host
@patch("api.ConnectApi.socket.gethostbyname")
def test_get_ip(mock_gethostbyname):
    mock_ip = "10.100.68.168"
    mock_gethostbyname.return_value = mock_ip
    expected = ConnectApi.get_ip()
    assert mock_ip == expected
    mock_gethostbyname.assert_called_once()
    
    
#Should get la fecha de creacion
@patch("api.ConnectApi.time.localtime")
@patch("api.ConnectApi.time.strftime")
def test_get_fecha(mock_time, mock_localtime):
    fecha_mock = "2022-07-01 00:00:00"
    mock_time.return_value = fecha_mock
    expected = ConnectApi.get_date()
    assert fecha_mock == expected
    mock_time.assert_called_once()
    mock_localtime.assert_called_once()

    
    
