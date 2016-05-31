@echo off
set /a buyer_count=1
set /a vendor_count=2

start Lab5\bin\Debug\Lab5.exe shop

for /l %%A in (1, 1, %vendor_count%) do (
	start Lab5\bin\Debug\Lab5.exe section
)


for /l %%B in (1, 1, %buyer_count%) do (
	start Lab5\bin\Debug\Lab5.exe buyer
)

pause

taskkill /im Lab5.exe /f