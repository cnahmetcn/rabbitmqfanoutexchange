# RabbitMQ
## RabbitMQ Nedir?
- RabbitMQ, mesaj kuyruk sistemidir. Yani gönderici elindeki veriyi RabbitMQ'ya gönderir ve RabbitMQ da sırası ve yeri geldiğinde ilgili alıcıya bu veriyi iletir. 
- Erland diliyle yazılmıştır.
- Open Source dur.
- AMQP protokolünü kullanır.
- **Message Broker**, mesaj kuyruk sistemlerine verilen genel bir isimdir. RabbitMQ dışında Azure Queue Storage, Azure Service Bus, Kafka ve MSMQ gibi message broker lar da vardır. 

## RabbitMQ Nasıl Çalışır?
![alt text](http://url/to/img.png)
- RabbitMQ içirisinde kuyruklar (**queue**) bulunmaktadır. Bu kuyruklar (**queue**) gelen mesajları taşıyan yapılardır. Kuyruğa ilk giren mesaj ilk çıkan olur. 
- RabbitMQ ya mesajları gönderen bir publisher var. Bu **publisher** herhangi bir uygulama olabilir. Node, JS, JAVA, mobil ... 
- **Publisher** elindeki mesajı direkt olarak kuyruğa da gönderebilir ya da gelişmiş bir yöntem olan **Exchange** e gönderir. Bir mesaj eğer **exchange** e geliyorsa, **exchange** gelen mesaj ilgili kuyuruklara routelama görevini yapar. Yani bir mesajı bir kuyruğa da gönderebilir, birden fazla kuyryğa da. Eğer **exchange** in routelayacağı herhangi bir kuyruk yoksa ilgili mesaj havada kalır, hiçbir şekilde bir kuyrukta tutulmadığından dolayı boşa gitmiş olur. 
- **Subscriber** ise RabbitMQ da sadece ilgili kuyruğu dinleyen uygulamadır. **Subscriber** da **Publisher** gibi herhangi bir uygulama olabilir. **Subscriber** lar bazen iki kuyruğuda dinleyebilir, bazen tek kuyruğu dinleyebilir, bazen de tek kuyruğu üç tane **Subscriber** da dinleyebilir. 

## RabbitMQ Niçin Kullanılmalıdır? 
- Request-Response süresini azaltmak için kullanılır. Herhangi bir uygulama için; resim ölçekleme, dosya formatı değiştirme... gibi uzun sürecek işlemleri RabbitMQ sayesinde ayrı bir senaryoda gerçekleştirebiliriz. Bu sayede kullanıcıyı bekletmez ve işlem arka planda yürütülür. 
- Microservice mimarilerde asenkron iletişim sağlamak için kullanılır. Bir microservice in diğer bir microservice ile haberleşmesi ya senkron yada asenkron olabilir. Senkron; bir microservice diğer bir microservice e istek yaptğında eğer response (sonuç) için bekliyorsa bu senkron iletişimdir. Bu teknolojiler restful veya gRPC olabilir. ASenkron; bir microservice diğer bir microservice e istek yaptğında eğer response (sonuç) için beklemiyorsa bu aseknron iletişimdir. Böyle durumlarda da RabbitMQ gibi çözümler devreye giriyor.  

RabbitMQ Cloud üzerinde çalışıyoruz.

### Subscriber dan Cevap Beklemek
- RabbitMQ ya artık mesajları hemen silmemesini, subscriber dan bir mesaj beklemesini ve ardından silmesini söyleyeceğiz. Bu özellik gelen mesajı doğru işleyene kadar ilgili mesajın kuyrukta beklemesini sağlar. 

### Birden fazla Subscriber Kullanımı
- Bir kuyruğa birden fazla subcsriber bağlanabilir. Yapmamız gereken RabbitMQ ya kuyruktaki mesajı, subscriber lara nasıl göndereceğimizi söylemek. Yani tek seferde 1 er 1 er  mi, yoksa 5 er 5 er mi gönderilecek. 
- Elimizde 10 mesaj ve 2 subscriber olsun (a ve b). Eğer 1 er 1 er göndermek istersek. 1-a 2-b 3-a 4-b 5-a 6-b 7-a 8-b 9-a 10-b şeklinde gider.
- Elimizde 10 mesaj ve 2 subscriber olsun (a ve b). Eğer 5 er 5 er göndermek istersek. (12345)-a ya (678910)-b ye gider.

## Exchange Types
- **Exchange**, Publisher'ın üretmiş olduğu mesajları ilgili kuyruklara route lamaktan görevli bir yapıdır. rabbitMQ da 4 farklı Exchange tipi vardır (Fanout, Direct, Topic, Header).

- Eğer Exchange kullanmazsak Default Exchange olarak adlandırılır.

### Fanout Exchange
- Publisher dan gelen mesajı herhangi bir filtreleme yapmadan kendisine bağlı olan tüm kuyruklara iletir. Aynı mesajı tüm kuyruklara gönderir. 

