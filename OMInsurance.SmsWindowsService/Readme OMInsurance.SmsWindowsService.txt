Установка сервиса смс.
	Желательно пересобрать проект.
	Необходимы все файлы лежащие в папке с OMInsurance.SmsWindowsService.exe
	Необходимы права администратора.

Найти где находится утилита installutil.exe 
обычно по пути: C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe

Выполнить команду в cmd из под админа
%путь к утилите%\installutil.exe  %путь к сервису%\OMInsurance.SmsWindowsService.exe
Например:
C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe C:\VS\OMInsurance\Sources\OMInsurance.SmsWindowsService\bin\Debug\OMInsurance.SmsWindowsService.exe

Зайти в сервисы и найти SmsWindowsService, зайти в свойства.
Перейти на 2ую вкладку -> переключатель перевести в "С системной учетной записью"

Запустить сервис.


Перед переустановкой или удалением сервиса, необходимо сначала остановить сервис!
удаление произодится такой же командой только с параметром /u
installutil.exe /u OMInsurance.SmsWindowsService.exe



