from turtle import position
import xlwings as xw
from xlwings import Range, constants
import HoockUtilities as helpers
import sys


#Should read config from json
def read_config(config_path):
    config= helpers.read_json(config_path)
    return config

#Should copy File path principal to move other path
def copy_template(path_config):
    try:
        config = read_config(path_config)
        date_execute = helpers.get_date_complete()
        path_config = helpers.dictToObject(config)

        if path_config:
            template_report = str(path_config.principal)+"reporte_ejecucion.xlsx"
            path_final = str(path_config.ruta_reporte)+"reporte_ejecucion_"+date_execute+".xlsx"
            helpers.copy_file(template_report,path_final)
            return path_final
    except IOError as error:
        except_info = sys.exc_info()
        s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
        helpers.put_log(s_message,"--","Report", "log_bot/Report.txt")


#Should change status of activate
def create_report(params):
    config_report = helpers.dictToObject(params)
    
    path_config_brand =  config_report.path
    sheet_name = config_report.sheet_name
    path_config = helpers.clear_folder_path(path_config_brand)
    
    try:
        with xw.App(visible=False) as app:
            book = app.books.open(r''+path_config, editable=True)
            sheet_book = get_sheet_name(book, sheet_name)
            last_row = get_last_row_book(book, sheet_book)
            next_row_write = int(last_row+1)
            
            for index, record_data in enumerate(config_report.data, next_row_write):
                position = str(index)
                write_row(sheet_book,"A"+position, record_data.fecha_ejecucion)
                write_row(sheet_book,"B"+str(index),record_data.month)
                write_row(sheet_book,"C"+str(index), record_data.year)                               
                write_row(sheet_book,"D"+str(index),record_data.marca)
                write_row(sheet_book,"E"+str(index), record_data.hour_init)
                write_row(sheet_book,"F"+str(index),record_data.hour_end)
                write_row(sheet_book,"G"+str(index),record_data.hour_execute)
                write_row(sheet_book,"H"+str(index), record_data.registros)
                write_row(sheet_book,"I"+str(index), record_data.observations)       
                       
            save_report(book)
            close_report(book)
            
    except IOError as error:
        except_info = sys.exc_info()
        s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
        helpers.put_log(s_message,"--","Report", "log_bot/Report.txt")


def get_last_row_book(book, sheet_book):
    ultima_fila = sheet_book.range('A' + str(book.sheets[0].cells.last_cell.row)).end('up').row
    return int(ultima_fila)

def get_sheet_name(book, sheet_name):
    return book.sheets[sheet_name]
  
def save_report(book):
    book.save()

def close_report(book):
    book.close()
    
def write_row(sheet_book, position, value_data):
    sheet_book[position].value = value_data

def read_cell(sheet_book, position):
    return sheet_book[position].value

#Cambio de estatus

def chance_status(brand_search:str, config:str, status:str):
    try:
        config = read_config(config)
        path_config = helpers.dictToObject(config)
        template_report = str(path_config.principal)+"configuracion.xlsx"
        path_config = helpers.clear_folder_path(template_report)
        
        with xw.App(visible=False) as app:
            book = app.books.open(r''+path_config, editable=True)
            sheet_book = get_sheet_name(book, "marcas")
            last_row = get_last_row_book(book, sheet_book)
            next_row_write = int(last_row+1)
            
            for index in range(2, next_row_write):
                position = "C"+str(index)
                brand = read_cell(sheet_book, position)
                if brand == brand_search.upper():
                    position = "B"+str(index)    
                    write_row(sheet_book, position, status)
                    
            save_report(book)
            close_report(book)
    except IOError as error:
        except_info = sys.exc_info()
        s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
        helpers.put_log(s_message,"--","Report", "log_bot/Report.txt")
