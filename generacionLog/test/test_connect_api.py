from api import ConnectApi

#Should connect with microservice 
def test_connect_api():
    params_mock = {}
    results = ConnectApi.connect()
    assert results == {}
    
#Should create body request used in microservice
def test_get_body_request():
    body_mock = {}
    expected_results = ConnectApi.get_body()
    assert expected_results == {}