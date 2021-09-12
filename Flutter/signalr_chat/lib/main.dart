import 'dart:io';
import 'package:http/io_client.dart';
import 'package:signalr_core/signalr_core.dart';

Future<String> getAccessToken() async {
  return "INFORME O TOKEN";
}

Future<void> main(List<String> arguments) async {
  final connection = HubConnectionBuilder()
      .withUrl(
          'http://192.168.0.108/SignalR.AspNetCore.POC/messages',
          HttpConnectionOptions(
            accessTokenFactory: () async => await getAccessToken(),
            client: IOClient(
                HttpClient()..badCertificateCallback = (x, y, z) => true),
            logging: (level, message) => print(message),
          ))
      .build();

  await connection.start();

  connection.on('ReceiveMessage', (message) {
    print(message.toString());
  });

  await connection
      .invoke('SendMessageToUser', args: ['ENTER USER ID', 'Says hi!']);
}
