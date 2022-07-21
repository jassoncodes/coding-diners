from unittest import mock
from api import ConnectApi

#Should connect with microservice 
def test_connect_api():
    params_mock = {}
    results = ConnectApi.connect()
    assert results == {}
    
#Should create body request used in microservice
@mock.patch("api.ConnectApi.get_uuid")
def test_get_body_request(get_uuid):
    uuid_mock = "773684d0-02b6-11ed-b939-0242ac120003"
    get_uuid.return_value = uuid_mock
    body_mock = { 
        "dinHeader": {
            "aplicacionId": "RPA",
            "canalId": "RPA",
            "uuid": uuid_mock
        }
    }
    expected_results = ConnectApi.get_body()
    assert expected_results == body_mock
    get_uuid.assert_called_once()
    
#Should generate uuid automatic
@mock.patch("api.ConnectApi.uuid.uuid4")
def test_get_uuid(mock_uuid4):
    uuid_mock = "773684d0-02b6-11ed-b939-0242ac120003"
    mock_uuid4.return_value = uuid_mock
    expected = ConnectApi.get_uuid()
    assert expected == uuid_mock
    mock_uuid4.assert_called_once()
    
    
