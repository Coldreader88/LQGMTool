@echo off
cd /d "%~dp0"
echo %1
echo ������Ϸ��bat��Ҫ��һ�£���zh-tw��Ϊzh-cn
GMTool.exe -tw2cn %1 %2 %3
pause