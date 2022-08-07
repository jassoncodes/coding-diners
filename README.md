# RPA_Fraudes_CDI

Scripts usuados para el los proyectos de RPA para el área de Fraudes.

Para poder realizar el debug del código, se debe considerar que se aplico TDD, para algunos de sus archivos.

* Ejecutar el entorno virutal, source venv/Script/activate
* Ejecutar en la linea de comandos los siguiente: pip install -r requiremnts.txt
* Correr los test pytest -v


Para la generación de los programas auxiliares desarrollados con lenguaje PYTHON, seguir los siguientes pasos:

1. Abrir el terminal, crear el entorno virtual, digitar el siguiente comando:
   1. python -m venv venv
2. Una vez abierto el proyecto y ejecutas el siguiente comando en Windows
   1. [ ] source venv/Script/activate.
   2. [ ] pip install pyinstaller.
   3. [ ] pyinstaller --onefile --windowed archivo principal.py
