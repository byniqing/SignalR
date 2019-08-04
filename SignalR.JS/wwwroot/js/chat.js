//连接服务器
var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5002/api/chatHub").build();

/*
监听ReceiveMessage方法
其实ReceiveMessage一个方法，有2个参数
*/
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
//启动
connection.start().catch(function (err) {
    return console.error(err.toString());
});

/*
 服务端是3.0之前，js客户端不会自动重连
 在3.0 之前, SignalR 的 JavaScript 客户端不会自动重新连接。 必须编写代码将手动重新连接你的客户端。
 */
//断开事件
connection.onclose(async () => {
    console.log("与服务器断开连接");
    await start();

});

//重连
async function start() {
    try {
        await connection.start();
        console.log("connected");
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000); //失败重连
    }
};
//绑定
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    /*
     远程调用服务的SendMessage方法，SendMessage方法有2 个参数
     即SendMessage(user,message)
     */
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

