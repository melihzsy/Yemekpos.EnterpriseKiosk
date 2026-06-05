# 🍔 Yemekpos Enterprise Kiosk

![Next.js](https://img.shields.io/badge/Next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)

Yemekpos Enterprise Kiosk, restoran zincirleri için tasarlanmış, tam donanımlı (Full-Stack), çok dilli ve yüksek performanslı bir self-servis sipariş kiosk uygulamasıdır. Müşteri deneyimini (UX) en üst düzeye çıkarırken, arka planda esnek ve ölçeklenebilir bir ilişkisel veritabanı mimarisi kullanır.

## ✨ Öne Çıkan Özellikler

* **🌍 Dinamik Çok Dilli Altyapı (i18n):** Sistem; Türkçe, İngilizce, Arapça ve Rusça dillerini destekler. Çeviriler "hardcoded" statik metinler yerine veritabanı (Seed Data) ve merkezi durum yönetimi (Zustand) üzerinden anlık olarak değişir.
* **🛒 Kesintisiz Sepet Yönetimi:** Zustand kütüphanesi ile oluşturulan `useCartStore` sayesinde ürün ekleme, çıkarma ve fiyat hesaplama işlemleri sayfa yenilenmeden, gerçek zamanlı gerçekleşir.
* **🎯 Akıllı Çapraz Satış (Upsell Modal):** Müşteri ödeme adımına geçmeden önce, sepet tutarını artırmaya yönelik "Son Dokunuş" algoritması ile ekstra sos ve içecek teklifleri sunulur.
* **💳 POS Simülasyonu & Dinamik Sipariş No:** Ödeme ekranında statik bir sayfa yerine, animasyonlu bir POS terminali simülasyonu ve her işlemde rastgele üretilen gerçekçi 3 haneli sipariş numaraları oluşturulur.
* **🔒 Idle Timeout (Hareketsizlik Güvenliği):** Kiosk başındaki müşteri işlemi yarım bırakırsa, sistem 20 saniyelik hareketsizlik sonrası 10 saniyelik bir geri sayım başlatır ve işlemi güvenli bir şekilde iptal ederek sepeti temizler.

## 🏗️ Mimari ve Teknolojiler

Proje, kurumsal standartlara (Enterprise-grade) uygun olarak birbirine gevşek bağlı (loosely coupled) iki ana katmandan oluşmaktadır:

### Frontend (İstemci)
* **Framework:** Next.js (React)
* **Dil:** TypeScript
* **State Management:** Zustand
* **Stil & Animasyon:** Tailwind CSS, Framer Motion

### Backend & Veritabanı (Sunucu)
* **Framework:** C# .NET Core Web API
* **ORM:** Entity Framework Core (Code-First Yaklaşımı)
* **Veritabanı:** PostgreSQL
* **Veritabanı Mimarisi (Relational Design):**
    * Ürünler ve Kategoriler arasında `One-to-Many` ilişki.
    * Sistemdeki opsiyonel seçimleri (Boyut, Sos, İçecek) koda gömmek yerine; `ModifierGroups` ve `Modifiers` tabloları kullanılmıştır.
    * **Junction (Köprü) Table:** Hangi ürünün hangi seçenekleri göstereceği `ProductModifierGroups` köprü tablosu üzerinden çoktan-çoka (Many-to-Many) ilişki ile yönetilmektedir.

## 🚀 Kurulum ve Çalıştırma

### Veritabanı ve Backend'i Ayağa Kaldırma
1. `Yemekpos.Enterprise.API` dizinine gidin.
2. `appsettings.json` dosyasındaki PostgreSQL bağlantı dizesini (Connection String) kendi bilgilerinize göre güncelleyin.
3. Terminalde aşağıdaki komutları çalıştırarak veritabanını oluşturun ve tohumlama (Seed) işlemini başlatın:
```bash
dotnet ef database update
dotnet run

Frontend'i Ayağa Kaldırma
frontend dizinine gidin.

Gerekli kütüphaneleri kurun ve geliştirici sunucusunu başlatın:

Bash
npm install
npm run dev
Tarayıcınızda http://localhost:3000 adresine giderek Kiosk'u deneyimleyin.

Geliştirici:
Melih Özsoy
Software Engineering Student @ Istanbul Nişantaşı University
