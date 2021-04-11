import json
import urllib.request
import ast

# ref https://stackoverflow.com/questions/9746303/how-do-i-send-a-post-request-as-a-json
wc = urllib.request.urlopen("http://api.openweathermap.org/data/2.5/weather?q=Copenhagen,dk&APPID=9b7e7338b21f570595cb4605ddfadbad").read().strip()
b = wc.decode("UTF-8")
mydata = ast.literal_eval(b)

weatherMain = mydata["weather"][0]["main"]

data = {"condition":weatherMain}

req = urllib.request.Request("http://localhost:8081/weatherCondition")    
req.add_header('Content-Type', 'application/json; charset=utf-8')
jsondata = json.dumps(data)
jsondataasbytes = jsondata.encode("utf-8")
req.add_header('Content-Length', len(jsondataasbytes))
urllib.request.urlopen(req, jsondataasbytes)