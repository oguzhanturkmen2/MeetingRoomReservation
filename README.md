# MeetingRoomReservation

Toplantı Odası Rezervasyon Sistemi (Hitsoft .NET Core Developer Case Çalışması)



1-Projeyi çalıştırma adımları



2-İş kuralları

&nbsp; a) Çakışan toplantı rezevasyonu yapılamaz. Bu şekilde olursa karmaşıklık olur.

&nbsp; b) Minumum rezervasyon  aralığı 30 dakika. Makul bir süre.

&nbsp; c) Maksimum rezervasyon  aralığı 4 saat. En fazla yarım gün toplantı olabilir. Sonrasında yemek arası veya iş çıkışı olur.

&nbsp; d) Geçmişe bir rezervasyon yapılamaz. Böyle bir şey anlamsız olur.

&nbsp; e) En fazla 3 ay ilerisine rezervasyon yapılabilir. Makul bir süre.

&nbsp; f) Rezervasyon iptali 15 dakika öncesine kadar yapılabilir. Mantıklı bir süre.

&nbsp; g) Başlamış bir toplantı iptal edilemez. Zaten toplantı saati geçmiş. Başka rezervasyon alınamaz.

&nbsp; h) Katılımcı sayısı oda kapasitesini aşamaz. Aşarsa hata verir ve rezervasyon yapmaz.

&nbsp; i) Bir kullanıcı aynı anda sadece bir odayı rezerve edebilir. Kullanıcı aynı anda iki yerde bulunamaz.

&nbsp; 

3-Tekrarlayan toplantılar
  Tekrarlayan toplantılar için ayrı bir tabloda toplantı bilgileri saklanır. Ayrıca rezervasyon tablosunda her bir tekrar için kayıt oluşturulur ve Tekrar tablosundaki ID rezervasyon tablosuna yazılır. Tekrarlayan toplantı iptal edildiğinde hem tekrarlayan tablosu hem de rezervasyon tablosundaki veriler silinir. Rezervasyon tablosunda da kayıt tutulduğu için herhangi bir çakışma durumu kolaylıkla bulunabilir.



4-Veritabanı şeması



5-API endpointler



