import xlwings as xw
import sys
import helpers
import datetime

try:
    if(len(sys.argv)>1):
        archivo_excel = sys.argv[1]
        nuevo_archivo = sys.argv[2]

        # Abrir la aplicación Excel
        app = xw.App(visible=False)  # Para abrir Excel en segundo plano sin mostrar la interfaz gráfica

        # Abrir el archivo Excel
        workbook = app.books.open(archivo_excel)

        # Obtener la hoja de trabajo activa
            #sheet_name = "FORMATO"
            #sheet = workbook.sheets[sheet_name]
        # Accessing the number of sheets in the workbook
        num_sheets = len(workbook.sheetnames)

        # Retrieving the last sheet by index
        last_sheet_index = num_sheets - 1
        last_sheet = workbook.worksheets[last_sheet_index]

        # Leer los datos de la hoja de trabajo
        data = sheet.range('B3').expand().value

        # Convertir las listas internas en tuplas
        data = [tuple(row) for row in data]

        # Obtener los valores únicos de la lista
        valores_unicos = set(data)

        # Crear un nuevo archivo Excel
        new_workbook = xw.Book()

        # Seleccionar la hoja de trabajo activa en el nuevo archivo
        new_sheet = new_workbook.sheets.active
        header_values = [
            "CasoID",
            "ID Principal",
            "Identificacion",
            "Nombre",
            "Descripcion Mitigación (antes)",
            "Descripcion Mitigación actual)",
            "Fecha Vencimiento",
            "Observación",
            "Delitos encontrados",
            "Comentario"
        ]

        header_range = new_sheet.range("A1:J1")  # Especifica el rango de la primera fila
        header_range.value = [header_values]  # Escribe los valores en el rango
        # Escribir los valores en el nuevo archivo Excel
        row_number = 2  # Variable para llevar el conteo de filas en el nuevo archivo

        for tupla in valores_unicos:
            if "Cédula" not in tupla:
                if all(value is not None and value != '' for value in tupla):
                    new_sheet.range(f'B{row_number}').number_format = '@'
                    new_sheet.range(f'B{row_number}').value = str(tupla[0]).strip()
                    new_sheet.range(f'C{row_number}').number_format = '@'
                    new_sheet.range(f'C{row_number}').value = str(tupla[1]).strip()
                    new_sheet.range(f'D{row_number}').number_format = '@'
                    new_sheet.range(f'D{row_number}').value = str(tupla[2]).strip()
                    print('RPA procesando: ', tupla)
                    row_number += 1

        # Guardar el nuevo archivo Excel
        new_workbook.save(nuevo_archivo)

        # Cerrar el nuevo archivo y el archivo original, y la aplicación
        new_workbook.close()
        # Cerrar el archivo Excel y la aplicación
        workbook.close()
        app.quit()

except IOError as error:
    except_info = sys.exc_info()
    s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
    helpers.put_log(s_message,"--","bsuquedaJuicios", f"LogTech_BUSQUEDAJUICIOS_{datetime.datetime.now():%Y%m%d}.xml")
    print('Giskard: ', 'bsuquedaJuicios ')

