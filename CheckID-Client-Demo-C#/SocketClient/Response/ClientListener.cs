﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginICAOClientSDK;
using PluginICAOClientSDK.Response.BiometricAuth;
using PluginICAOClientSDK.Response.CardDetectionEvent;
using PluginICAOClientSDK.Response.GetDocumentDetails;

namespace ClientInspectionSystem.SocketClient.Response {
    public class ClientListener : ISPluginClient.ISListener {
        public void doSend(string cmd, string id, ISMessage<object> data) {
            throw new NotImplementedException();
        }

        public void onConnectDenied() {
            throw new NotImplementedException();
        }

        public void onConnected() {
            throw new NotImplementedException();
        }

        public void onDisconnected() {
            throw new NotImplementedException();
        }

        public void onPreConnect() {
            throw new NotImplementedException();
        }

        public void onReceive(string cmd, string id, int error, ISMessage<object> data) {
            throw new NotImplementedException();
        }

        public bool onReceivedBiometricResult(BiometricAuthResp baseBiometricAuth) {
            throw new NotImplementedException();
        }

        public bool onReceivedDocument(DocumentDetailsResp document) {
            throw new NotImplementedException();
        }

        public bool onReceviedCardDetectionEvent(CardDetectionEventResp baseCardDetectionEvent) {
            throw new NotImplementedException();
        }
    }
}
