Сделано:
- рефакторинг архитектуры сцены и префабов
- рефакторинг неймингов и архитекртуры:
	- изменил логику именования полей и методов
	- добавил базовые классы BaseTower, BaseProjectile
	- добавил интерфейсы для поведения башен (IShootable, IRotatable), снарядов (IPoolable), пулов объектов (IObjectPool)
	- инкапсулировал поля классов
- исключил тяжелые циклы с методов FindObjectsOfType из Update'ов
- добавил конфиг GameConfig для удобной настройки всех параметров
- добавил поворот башни и стрельбу на упреждение с ожиданием завершения поворота

Надо сделать:
- ITargetingStrategy для обеих башен
- ICanShootingStrategy для обеих башен
- че-то сделать с GetRangeToFindEnemy

<img width="1847" height="963" alt="image" src="https://github.com/user-attachments/assets/bfbecb05-92c1-43cf-8f11-e40bf3fb29ac" />
