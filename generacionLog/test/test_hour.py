from manager  import Hour
from unittest.mock import patch
from unittest import mock

@patch("manager.Hour.datetime")
def test_get_diference_hour(mock_datetime):
    expected = Hour.get_diference_hour()
