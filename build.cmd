@echo off
cd %~dp0
SETLOCAL
call npm install -g gulp
call npm install
call gulp build