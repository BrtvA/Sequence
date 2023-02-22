# Sequence
Программа предназначена для получения битовой последовательности из временных рядов и последующей проверки получившейся последовательности при помощи набора статистических тестов [NIST 800-22](https://nvlpubs.nist.gov/nistpubs/legacy/sp/nistspecialpublication800-22r1a.pdf).

Инструкция:

1) Выберете режим работы: Intensity fluctuation/Bit sequence;
2) Откройте исследуемый csv/txt файл;
3) При необходимости заполните поля: ADS's bit's, Shift time, Number of LSB's, Time series. Активация чек-бокса Time series активирует ручной режим настройки длины временного ряда;
4) Активируйте необходимые тесты и нажмите кнопку Execute Test. Для активации всех тестов нажмите кнопку Select All Test's;
5) Дождитесь окончания выполнения тестов;
6) Чтобы сохранить битовую последовательность в txt файл, нажмите Save sequence;
6) Для очистки результатов выполнения тестов нажмите Reset.

Цветовая индикация результатов выполнения: зеленый - тест пройден; красный - тест не пройден; желтый - не выполнены начальные условия для запуска теста.

![Alt text](DataAndImage/screen%20shot.png)

Для удобства работы поместите exe файл программы в отдельную папку, поскольку в процессе работы создается json файл с пользовательскими настройками.
