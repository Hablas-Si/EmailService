To the test the endpoint and send a email.

1. Git pull the service, this will launch the docker container on your computer.
2. Then run a "dotnet run" command in your terminal in the folder with the solution file. 
3. Next you can simply go to rabbitMQ management 
4. Login with guest, guest 
5. Access tab called "queues and streams"
6. Click "Email queue"
7. Insert into payload with this Json format
{
  "To": "recipient@example.com",
  "Subject": "Test Email from RabbitMQ",
  "Body": "This is a test email sent from RabbitMQ."
}
8. Then simply click publish message, you can now see in your terminal the message queue has been received and sent.
 
You can also try to enter your own email into the Json format above, if you want to physically receive the email.
