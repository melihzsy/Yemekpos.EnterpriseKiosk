using System.Collections.Generic;
using System.Linq;
using Yemekpos.Enterprise.API.Models;

namespace Yemekpos.Enterprise.API.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        if (context.Categories.Any() || context.ModifierGroups.Any() || context.Modifiers.Any() || context.Products.Any() || context.ProductModifierGroups.Any()) return;

        var categories = new[]
        {
            new Category { NameTr = "Avantajlı Menüler", NameEn = "Value Menus", NameAr = "وجبات التوفير", NameRu = "Выгодные меню", IsActive = true },
            new Category { NameTr = "Tekli Burgerler", NameEn = "Single Burgers", NameAr = "برجر مفرد", NameRu = "Одиночные бургеры", IsActive = true },
            new Category { NameTr = "Pizzalar", NameEn = "Pizzas", NameAr = "بيتزا", NameRu = "Пицца", IsActive = true },
            new Category { NameTr = "Çıtır Lezzetler", NameEn = "Crispy Bites", NameAr = "وجبات مقرمشة", NameRu = "Хрустящие закуски", IsActive = true },
            new Category { NameTr = "İçecekler", NameEn = "Drinks", NameAr = "مشروبات", NameRu = "Напитки", IsActive = true },
            new Category { NameTr = "Tatlılar", NameEn = "Desserts", NameAr = "حلويات", NameRu = "Десерты", IsActive = true }
        };
        context.Categories.AddRange(categories);

        var groups = new[]
        {
            new ModifierGroup { NameTr = "İçecek Seçimi", NameEn = "Drink Choice", NameAr = "اختيار المشروب", NameRu = "Выбор напитка", MinSelect = 1, MaxSelect = 1 },
            new ModifierGroup { NameTr = "Boyut Seçimi", NameEn = "Size Choice", NameAr = "اختيار الحجم", NameRu = "Выбор размера", MinSelect = 1, MaxSelect = 1 },
            new ModifierGroup { NameTr = "Ekstra Malzemeler", NameEn = "Extra Ingredients", NameAr = "مكونات إضافية", NameRu = "Дополнительные ингредиенты", MinSelect = 0, MaxSelect = 4 },
            new ModifierGroup { NameTr = "Soslar", NameEn = "Sauces", NameAr = "صلصات", NameRu = "Соусы", MinSelect = 0, MaxSelect = 3 }
        };
        context.ModifierGroups.AddRange(groups);
        context.SaveChanges();

        var categoryIds = context.Categories.ToDictionary(x => x.NameEn, x => x.Id);
        var groupIds = context.ModifierGroups.ToDictionary(x => x.NameEn, x => x.Id);

        Modifier M(string g, string tr, string en, string ar, string ru, string url, decimal p = 0) =>
            new() { ModifierGroupId = groupIds[g], NameTr = tr, NameEn = en, NameAr = ar, NameRu = ru, ImageUrl = url, ExtraPrice = p, IsActive = true };

        context.Modifiers.AddRange(
            M("Drink Choice","Kola","Cola","كولا","Кола","https://images.pexels.com/photos/2668308/pexels-photo-2668308.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Drink Choice","Şekersiz Kola","Zero Cola","كولا زيرو","Кола Зеро","https://images.pexels.com/photos/3631/glass-drink-coca-cola.jpg?auto=compress&cs=tinysrgb&w=1200"),
            M("Drink Choice","Fanta","Orange Soda","فانتا","Фанта","https://images.pexels.com/photos/2109099/pexels-photo-2109099.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Drink Choice","Sprite","Lemon Soda","سبرايت","Спрайт","https://images.pexels.com/photos/96974/pexels-photo-96974.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Drink Choice","Ayran","Ayran","عيران","Айран","https://images.pexels.com/photos/8931189/pexels-photo-8931189.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Drink Choice","Şeftali Ice Tea","Peach Iced Tea","شاي مثلج بالخوخ","Персиковый холодный чай","https://images.pexels.com/photos/2103949/pexels-photo-2103949.jpeg?auto=compress&cs=tinysrgb&w=1200",2),
            M("Drink Choice","Limonata","Lemonade","ليموناضة","Лимонад","https://images.pexels.com/photos/952360/pexels-photo-952360.jpeg?auto=compress&cs=tinysrgb&w=1200",3),
            M("Drink Choice","Soğuk Kahve","Iced Coffee","قهوة مثلجة","Холодный кофе","https://images.pexels.com/photos/312418/pexels-photo-312418.jpeg?auto=compress&cs=tinysrgb&w=1200",6),
            M("Size Choice","Küçük Boy","Small Size","حجم صغير","Маленький размер","https://images.pexels.com/photos/4465124/pexels-photo-4465124.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Size Choice","Orta Boy","Medium Size","حجم وسط","Средний размер","https://images.pexels.com/photos/139746/pexels-photo-139746.jpeg?auto=compress&cs=tinysrgb&w=1200",10),
            M("Size Choice","Büyük Boy","Large Size","حجم كبير","Большой размер","https://images.pexels.com/photos/338713/pexels-photo-338713.jpeg?auto=compress&cs=tinysrgb&w=1200",20),
            M("Size Choice","Mega Boy","Mega Size","حجم ميجا","Мега размер","https://images.pexels.com/photos/3620705/pexels-photo-3620705.jpeg?auto=compress&cs=tinysrgb&w=1200",30),
            M("Extra Ingredients","Cheddar Peyniri","Cheddar Cheese","جبنة شيدر","Сыр Чеддер","https://images.pexels.com/photos/821365/pexels-photo-821365.jpeg?auto=compress&cs=tinysrgb&w=1200",14),
            M("Extra Ingredients","Mozzarella","Mozzarella","موزاريلا","Моцарелла","https://images.pexels.com/photos/1435907/pexels-photo-1435907.jpeg?auto=compress&cs=tinysrgb&w=1200",14),
            M("Extra Ingredients","Jalapeno","Jalapeno","هالبينو","Халапеньо","https://images.pexels.com/photos/5945648/pexels-photo-5945648.jpeg?auto=compress&cs=tinysrgb&w=1200",8),
            M("Extra Ingredients","Karamelize Soğan","Caramelized Onion","بصل مكرمل","Карамелизованный лук","https://images.pexels.com/photos/533342/pexels-photo-533342.jpeg?auto=compress&cs=tinysrgb&w=1200",7),
            M("Extra Ingredients","Mantar","Mushroom","فطر","Грибы","https://images.pexels.com/photos/375889/pexels-photo-375889.jpeg?auto=compress&cs=tinysrgb&w=1200",9),
            M("Extra Ingredients","Dana Bacon","Beef Bacon","لحم بقري مقدد","Говяжий бекон","https://images.pexels.com/photos/1927377/pexels-photo-1927377.jpeg?auto=compress&cs=tinysrgb&w=1200",18),
            M("Extra Ingredients","Acı Biber","Hot Pepper","فلفل حار","Острый перец","https://images.pexels.com/photos/1435895/pexels-photo-1435895.jpeg?auto=compress&cs=tinysrgb&w=1200",6),
            M("Extra Ingredients","Turşu","Pickles","مخلل","Соленья","https://images.pexels.com/photos/2689419/pexels-photo-2689419.jpeg?auto=compress&cs=tinysrgb&w=1200",5),
            M("Extra Ingredients","Ekstra Köfte","Extra Patty","شريحة لحم إضافية","Дополнительная котлета","https://images.pexels.com/photos/1639562/pexels-photo-1639562.jpeg?auto=compress&cs=tinysrgb&w=1200",28),
            M("Extra Ingredients","Ekstra Tavuk Fileto","Extra Chicken Fillet","فيليه دجاج إضافي","Дополнительное куриное филе","https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=1200",24),
            M("Sauces","Ketçap","Ketchup","كاتشب","Кетчуп","https://images.pexels.com/photos/5947011/pexels-photo-5947011.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Sauces","Mayonez","Mayonnaise","مايونيز","Майонез","https://images.pexels.com/photos/1437272/pexels-photo-1437272.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            M("Sauces","Barbekü Sos","BBQ Sauce","صلصة باربيكيو","Соус барбекю","https://images.pexels.com/photos/4109948/pexels-photo-4109948.jpeg?auto=compress&cs=tinysrgb&w=1200",5),
            M("Sauces","Ranch Sos","Ranch Sauce","صلصة رانش","Соус Ранч","https://images.pexels.com/photos/1437268/pexels-photo-1437268.jpeg?auto=compress&cs=tinysrgb&w=1200",5),
            M("Sauces","Acı Sos","Hot Sauce","صلصة حارة","Острый соус","https://images.pexels.com/photos/1435904/pexels-photo-1435904.jpeg?auto=compress&cs=tinysrgb&w=1200",4),
            M("Sauces","Sarımsaklı Mayonez","Garlic Mayo","مايونيز بالثوم","Чесночный майонез","https://images.pexels.com/photos/1437267/pexels-photo-1437267.jpeg?auto=compress&cs=tinysrgb&w=1200",4),
            M("Sauces","Bal Hardal","Honey Mustard","خردل بالعسل","Медово-горчичный соус","https://images.pexels.com/photos/4109946/pexels-photo-4109946.jpeg?auto=compress&cs=tinysrgb&w=1200",5),
            M("Sauces","Buffalo Sos","Buffalo Sauce","صلصة بافالو","Соус Баффало","https://images.pexels.com/photos/616354/pexels-photo-616354.jpeg?auto=compress&cs=tinysrgb&w=1200",6),
            M("Sauces","Tartar Sos","Tartar Sauce","صلصة تارتار","Соус Тартар","https://images.pexels.com/photos/5947009/pexels-photo-5947009.jpeg?auto=compress&cs=tinysrgb&w=1200",5),
            M("Sauces","Özel Burger Sosu","Special Burger Sauce","صلصة برجر خاصة","Специальный бургерный соус","https://images.pexels.com/photos/1586947/pexels-photo-1586947.jpeg?auto=compress&cs=tinysrgb&w=1200",7)
        );

        Product P(string c, string tr, string en, string ar, string ru, decimal price, string image) => new() { CategoryId = categoryIds[c], NameTr = tr, NameEn = en, NameAr = ar, NameRu = ru, Price = price, ImageUrl = image, IsActive = true };
        
        context.Products.AddRange(
            P("Value Menus","Texas Burger Menü","Texas Burger Menu","وجبة تكساس برجر","Техас Бургер Меню",259,"https://images.pexels.com/photos/2983101/pexels-photo-2983101.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Big Boss Burger Menü","Big Boss Burger Menu","وجبة بيج بوس برجر","Меню Биг Босс Бургер",279,"https://images.pexels.com/photos/1639562/pexels-photo-1639562.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Cheddar Melt Menü","Cheddar Melt Menu","وجبة تشيدر ميلت","Меню Чеддер Мелт",269,"https://images.pexels.com/photos/3616956/pexels-photo-3616956.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Mega Chicken Menü","Mega Chicken Menu","وجبة ميجا تشيكن","Меню Мега Чикен",262,"https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Ultimate Smoke Menü","Ultimate Smoke Menu","وجبة ألتميت سموك","Меню Ультимейт Смоук",288,"https://images.pexels.com/photos/1639557/pexels-photo-1639557.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Kids Burger Menü","Kids Burger Menu","وجبة برجر أطفال","Детское Бургер Меню",199,"https://images.pexels.com/photos/3219483/pexels-photo-3219483.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Duble Köfte Menü","Double Patty Menu","وجبة دبل كفتة","Меню Двойная Котлета",289,"https://images.pexels.com/photos/6605214/pexels-photo-6605214.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Value Menus","Acılı Fiesta Menü","Spicy Fiesta Menu","وجبة فييستا حارة","Острое Фиеста Меню",274,"https://images.pexels.com/photos/6605211/pexels-photo-6605211.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Klasik Burger","Classic Burger","برجر كلاسيكي","Классический бургер",145,"https://images.pexels.com/photos/1639557/pexels-photo-1639557.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Double Cheese Burger","Double Cheese Burger","دبل تشيز برجر","Двойной чизбургер",189,"https://images.pexels.com/photos/6605214/pexels-photo-6605214.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","BBQ Smoke Burger","BBQ Smoke Burger","باربيكيو سموك برجر","Барбекю Смоук Бургер",198,"https://images.pexels.com/photos/2983101/pexels-photo-2983101.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Meksika Acılı Burger","Mexican Spicy Burger","برجر مكسيكي حار","Мексиканский острый бургер",192,"https://images.pexels.com/photos/6605211/pexels-photo-6605211.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Tavuk Burger","Chicken Burger","برجر دجاج","Куриный бургер",154,"https://images.pexels.com/photos/3130875/pexels-photo-3130875.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Crispy Tavuk Burger","Crispy Chicken Burger","برجر دجاج مقرمش","Хрустящий куриный бургер",168,"https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Mantar Burger","Mushroom Burger","برجر فطر","Грибной бургер",176,"https://images.pexels.com/photos/3130875/pexels-photo-3130875.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Single Burgers","Cheese Lava Burger","Cheese Lava Burger","تشيز لافا برجر","Чиз Лава Бургер",199,"https://images.pexels.com/photos/1146760/pexels-photo-1146760.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Margarita Pizza","Margherita Pizza","بيتزا مارغريتا","Пицца Маргарита",210,"https://images.pexels.com/photos/825661/pexels-photo-825661.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Karışık Pizza","Supreme Pizza","بيتزا مشكلة","Пицца Суприм",245,"https://images.pexels.com/photos/1146760/pexels-photo-1146760.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Pepperoni Pizza","Pepperoni Pizza","بيتزا بيبروني","Пицца Пепперони",238,"https://images.pexels.com/photos/2619967/pexels-photo-2619967.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Dört Peynirli Pizza","Four Cheese Pizza","بيتزا أربع أجبان","Пицца Четыре Сыра",252,"https://images.pexels.com/photos/4109126/pexels-photo-4109126.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Etli Pizza","Meat Lovers Pizza","بيتزا عشاق اللحوم","Мясная пицца",268,"https://images.pexels.com/photos/2147491/pexels-photo-2147491.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Sebzeli Pizza","Veggie Pizza","بيتزا خضار","Вегетарианская пицца",224,"https://images.pexels.com/photos/1435904/pexels-photo-1435904.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Barbekü Tavuk Pizza","BBQ Chicken Pizza","بيتزا دجاج باربيكيو","Пицца с курицей барбекю",259,"https://images.pexels.com/photos/708587/pexels-photo-708587.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Pizzas","Acılı Sucuk Pizza","Spicy Sausage Pizza","بيتزا سجق حار","Пицца с острой колбасой",249,"https://images.pexels.com/photos/1566837/pexels-photo-1566837.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","6'lı Çıtır Tavuk","6 pcs Crispy Chicken","6 قطع دجاج مقرمش","6 шт. Хрустящая курица",149,"https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","9'lu Çıtır Tavuk","9 pcs Crispy Chicken","9 قطع دجاج مقرمش","9 шт. Хрустящая курица",199,"https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","12'li Çıtır Tavuk","12 pcs Crispy Chicken","12 قطع دجاج مقرمش","12 шт. Хрустящая курица",244,"https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","Mozzarella Stick","Mozzarella Sticks","أصابع موزاريلا","Сыроные палочки Моцарелла",129,"https://images.pexels.com/photos/4109084/pexels-photo-4109084.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","Soğan Halkası","Onion Rings","حلقات بصل","Луковые кольца",112,"https://images.pexels.com/photos/704569/pexels-photo-704569.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","Patates Kızartması","French Fries","بطاطس مقلية","Картофель фри",89,"https://images.pexels.com/photos/1583884/pexels-photo-1583884.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","Nugget Kutu","Nugget Box","علبة ناجتس","Наггетсы",159,"https://images.pexels.com/photos/3219483/pexels-photo-3219483.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Crispy Bites","Çıtır Mix Tabak","Crispy Mix Plate","طبق مكس مقرمش","Тарелка хрустящего микса",229,"https://images.pexels.com/photos/616354/pexels-photo-616354.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Kola 33cl","Cola 33cl","كولا 330 مل","Кола 0.33л",45,"https://images.pexels.com/photos/2668308/pexels-photo-2668308.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Kola 1L","Cola 1L","كولا 1 لتر","Кола 1л",74,"https://images.pexels.com/photos/3631/glass-drink-coca-cola.jpg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Ayran 30cl","Ayran 30cl","عيران 300 مل","Айран 0.3л",39,"https://images.pexels.com/photos/8931189/pexels-photo-8931189.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Limonata","Lemonade","ليموناضة","Лимонад",49,"https://images.pexels.com/photos/952360/pexels-photo-952360.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Meyveli Soda","Fruit Soda","صودا فواكه","Фруктовая газировка",42,"https://images.pexels.com/photos/616833/pexels-photo-616833.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Soğuk Kahve","Iced Coffee","قهوة مثلجة","Холодный кофе",62,"https://images.pexels.com/photos/312418/pexels-photo-312418.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Drinks","Milkshake","Milkshake","ميلك شيك","Милкшейк",78,"https://images.pexels.com/photos/3727250/pexels-photo-3727250.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Sufle","Chocolate Souffle","سوفليه شوكولاتة","Шоколадное суфле",109,"https://images.pexels.com/photos/291528/pexels-photo-291528.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Cheesecake","Cheesecake","تشيز كيك","Чизкейк",124,"https://images.pexels.com/photos/291528/pexels-photo-291528.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Tiramisu","Tiramisu","تيراميسو","Тирамису",132,"https://images.pexels.com/photos/6880219/pexels-photo-6880219.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Dondurma","Ice Cream","آيس كريم","Мороженое",69,"https://images.pexels.com/photos/1352278/pexels-photo-1352278.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Çikolatalı Kurabiye","Chocolate Cookie","كوكيز شوكولاتة","Шоколадное печенье",49,"https://images.pexels.com/photos/230325/pexels-photo-230325.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Waffle","Waffle","وافل","Вафли",139,"https://images.pexels.com/photos/376464/pexels-photo-376464.jpeg?auto=compress&cs=tinysrgb&w=1200"),
            P("Desserts","Mini Donut","Mini Donut","ميني دونات","Мини пончики",72,"https://images.pexels.com/photos/4686814/pexels-photo-4686814.jpeg?auto=compress&cs=tinysrgb&w=1200")
        );
        context.SaveChanges();

        var pids = context.Products.ToDictionary(x => x.NameEn, x => x.Id);
        var links = new List<ProductModifierGroup>();
        
        foreach (var name in new[] { "Texas Burger Menu","Big Boss Burger Menu","Cheddar Melt Menu","Mega Chicken Menu","Ultimate Smoke Menu","Kids Burger Menu","Double Patty Menu","Spicy Fiesta Menu" })
        { 
            links.Add(new ProductModifierGroup { ProductId = pids[name], ModifierGroupId = groupIds["Drink Choice"] }); 
            links.Add(new ProductModifierGroup { ProductId = pids[name], ModifierGroupId = groupIds["Size Choice"] }); 
            links.Add(new ProductModifierGroup { ProductId = pids[name], ModifierGroupId = groupIds["Sauces"] }); 
        }
        
        foreach (var name in new[] { "Classic Burger","Double Cheese Burger","BBQ Smoke Burger","Mexican Spicy Burger","Chicken Burger","Crispy Chicken Burger","Mushroom Burger","Cheese Lava Burger","Margherita Pizza","Supreme Pizza","Pepperoni Pizza","Four Cheese Pizza","Meat Lovers Pizza","Veggie Pizza","BBQ Chicken Pizza","Spicy Sausage Pizza","6 pcs Crispy Chicken","9 pcs Crispy Chicken","12 pcs Crispy Chicken","Mozzarella Sticks","Onion Rings","French Fries","Nugget Box","Crispy Mix Plate" })
        { 
            links.Add(new ProductModifierGroup { ProductId = pids[name], ModifierGroupId = groupIds["Extra Ingredients"] });
            links.Add(new ProductModifierGroup { ProductId = pids[name], ModifierGroupId = groupIds["Sauces"] }); 
        }
        
        context.ProductModifierGroups.AddRange(links);
        context.SaveChanges();
    }
}