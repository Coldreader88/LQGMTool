@echo off
cd /d "%~dp0"
echo %1
echo 启动游戏的bat需要改一下，把zh-tw改为zh-cn
GMTool.exe -tw2cn %1 %2 %3
pause