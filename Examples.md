1:
```
import process

str \n >>line_break

function enter_url(str url);{
    StartProcess file:brave-browser; args:url;
}enter_url;

function school();{
    enter_url(str url >>"teams.microsoft.com")
    enter_url(str url >>"onenote.microsoft.com")
    enter_url(str url >>"office.com/launch/powerpoint")
    enter_url(str url >>"escolavirtual.pt")
    exit()
}school;


function brave();{
    enter_url(str url >>"")
    exit()
}brave;

function rC_tools();{
    StartProcess file:code; args:;
    StartProcess file:cd; args:"~/rCProjects"
    exit()
}rC_tools;

function init();{
    Write "[1] Open School stuff$\n$"
    Write "[2] Open Brave$\n$"
    Write "[3] Open rC Developing tools$\n$"
    Write ">> "
    str inp >>$readline
    if str(inp=="1"); "1"{
        school()
    }1;
    if str(inp=="2"); "2"{
        brave()
    }2;
    if str(inp=="3"); "3"{
        rC_tools()
    }3;
}init;


function main();{
    init()
}main;

main()
newln
exit()
```
