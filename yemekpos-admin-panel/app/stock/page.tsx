"use client";

import React, { useEffect, useState } from "react";
import { Package, Save, Search } from "lucide-react";

interface Product {
  id: number;
  nameTr: string;
  imageUrl: string;
  stockBalance: number;
}

export default function StockPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  
  // Her ürünün stok input değerini ayrı ayrı tutmak için
  const [stockInputs, setStockInputs] = useState<{ [key: number]: number }>({});
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const res = await fetch("http://localhost:5198/api/Products");
      if (res.ok) {
        const data = await res.json();
        setProducts(data.sort((a: Product, b: Product) => a.id - b.id));
        
        // Tablo ilk yüklendiğinde inputların içine veritabanındaki güncel stokları yaz
        const initialInputs: { [key: number]: number } = {};
        data.forEach((p: Product) => {
          initialInputs[p.id] = p.stockBalance;
        });
        setStockInputs(initialInputs);
      }
      setLoading(false);
    } catch (error) {
      console.error("Ürünler çekilemedi:", error);
      setLoading(false);
    }
  };

  const handleStockChange = (id: number, value: string) => {
    setStockInputs({ ...stockInputs, [id]: parseInt(value) || 0 });
  };

  const handleSaveStock = async (id: number) => {
    try {
      const newStock = stockInputs[id];
      const res = await fetch(`http://localhost:5198/api/Products/stock/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        // Sadece yazdığımız DTO'nun beklediği verileri yolluyoruz
        body: JSON.stringify({ id: id, stockBalance: newStock }), 
      });

      if (res.ok) {
        alert("Stok başarıyla güncellendi!");
        fetchProducts(); // Tabloyu yeni verilerle yenile
      } else {
        alert("Güncelleme sırasında bir hata oluştu.");
      }
    } catch (error) {
      console.error("Stok güncelleme hatası:", error);
    }
  };

  // ... handleSaveStock fonksiyonunun bittiği yer ...

  // ARAMA FİLTRESİ: Sadece aranan kelimeyi içeren ürünleri süzer
  const filteredProducts = products.filter((product) =>
    product.nameTr.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading) return <div className="flex justify-center items-center h-screen text-xl font-semibold">Yükleniyor...</div>;
 

  return (
    <div className="min-h-screen bg-slate-50 p-8">
      <div className="max-w-6xl mx-auto bg-white rounded-2xl shadow-xl overflow-hidden border border-slate-100">
        
        {/* YENİ ÜST KISIM VE ARAMA ÇUBUĞU */}
        <div className="p-6 bg-emerald-700 text-white flex flex-col md:flex-row justify-between items-center gap-4">
          <div className="flex items-center gap-3">
            <Package className="w-6 h-6 text-emerald-300" />
            <h1 className="text-2xl font-bold tracking-wide">Depo ve Stok Yönetimi</h1>
          </div>
          
          {/* Arama Kutusu */}
          <div className="relative w-full md:w-72">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Search className="h-5 w-5 text-emerald-300/70" />
            </div>
            <input
              type="text"
              placeholder="Stokta Ara (Örn: Patates)..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="block w-full pl-10 pr-3 py-2 border border-emerald-600/50 rounded-xl leading-5 bg-emerald-800/50 text-white placeholder-emerald-300/50 focus:outline-none focus:bg-white focus:text-slate-900 focus:ring-2 focus:ring-indigo-500 transition-all sm:text-sm"
            />
          </div>
        </div>


        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-slate-100 text-slate-600 border-b border-slate-200 text-sm uppercase tracking-wider">
                <th className="p-4 font-semibold">Görsel</th>
                <th className="p-4 font-semibold">Ürün Adı</th>
                <th className="p-4 font-semibold text-center">Mevcut Stok</th>
                <th className="p-4 font-semibold text-center">Yeni Stok Gir</th>
                <th className="p-4 font-semibold text-center">İşlem</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {filteredProducts.map((product) => (
                <tr key={product.id} className="hover:bg-slate-50 transition-colors">
                  <td className="p-4">
                    <img src={product.imageUrl} alt={product.nameTr} className="w-12 h-12 object-cover rounded-lg shadow-sm border border-slate-200" />
                  </td>
                  <td className="p-4 font-medium text-slate-800">{product.nameTr}</td>
                  
                  {/* Stok 10'un altındaysa kırmızı, değilse yeşil yanan dinamik tasarım */}
                  <td className="p-4 text-center">
                    <span className={`px-3 py-1 rounded-full font-bold text-sm ${product.stockBalance < 10 ? 'bg-red-100 text-red-600' : 'bg-emerald-100 text-emerald-600'}`}>
                      {product.stockBalance} Adet
                    </span>
                  </td>
                  
                  <td className="p-4 text-center">
                    <input
                      type="number"
                      value={stockInputs[product.id] ?? ""}
                      onChange={(e) => handleStockChange(product.id, e.target.value)}
                      className="w-24 border-2 border-slate-200 rounded-lg p-2 text-center focus:border-emerald-500 focus:outline-none"
                    />
                  </td>
                  
                  <td className="p-4 text-center">
                    <button
                      onClick={() => handleSaveStock(product.id)}
                      className="inline-flex items-center gap-2 bg-emerald-600 hover:bg-emerald-700 text-white px-4 py-2 rounded-lg font-medium transition-colors shadow-sm active:scale-95"
                    >
                      <Save className="w-4 h-4" /> Güncelle
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}