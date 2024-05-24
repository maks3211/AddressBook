Simple address book .net core app
<br>Use  https://localhost:7004/swagger/index.html  to controll all methods. 

<br>
To get last added address into book:  /Book/getLast
<br>To get all object filter by parameter use: /ObjectName/show/{by}={val}
<br>eg. /Book/show/cityid=1

<br>Data is stored in memory. Functions that allow reading and saving froam a .json file are also available.

<br>Created unit tests for all models and e2e tests for CityController.
