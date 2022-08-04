import time
import sys
import smtplib, ssl
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
import HoockUtilities as helpers


def sender_email(config_path, email_sender):
    try:
        email = helpers.dictToObject(email_sender)
        config_email = helpers.get_data_json(config_path)
        path_email = config_email.email_config
        params_config = helpers.get_data_json(path_email)
        sender_address = params_config.username
        sender_password = params_config.password
        sender_host =  params_config.host
        sender_port = int(params_config.port)
        email_list = helpers.get_data_json(config_email.email_list)

        receiver_address = email_list.listEmail

        #Setup the MIME
        message = MIMEMultipart()
        start_time = time.time()

        message['Subject'] = str(email.subject)  #The subject line
        mail_content = str(email.content)

        message['From'] = sender_address

        message.attach(MIMEText(mail_content, 'html'))

        for email_to in receiver_address:
            message['To'] = email_to
            # context = ssl.create_default_context()
            # with smtplib.SMTP(sender_host, sender_port) as server:
            #     server.ehlo()  # Can be omitted
            #     server.starttls(context=context)
            #     server.ehlo()  # Can be omitted
            #     email_message = message.as_string()
            #     server.login(sender_address, sender_password)
            #     server.sendmail(sender_address, email_to, email_message)
            sender_context = ssl.create_default_context()
            with smtplib.SMTP_SSL(sender_host, sender_port, context=sender_context) as session:
                email_message = message.as_string()
                session.sendmail(sender_address, email_to, email_message)
                session.quit()
                time_process = str(round((time.time() - start_time),2))
                s_message = "Mensaje enviado: "+time_process+" "
                helpers.put_log(s_message,"--","Email", "log_bot/Email.txt")

        
    except ValueError  as error:
        except_info = sys.exc_info()
        s_message = f'({except_info[2].tb_lineno}) {except_info[0]} {str(error)}'
        helpers.put_log(s_message,"--","Email", "log_bot/Email.txt")
