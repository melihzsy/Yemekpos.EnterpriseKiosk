"use client";

import React, { useEffect, useState } from "react";
import { Pencil, Check, X, Tag, Search } from "lucide-react";

interface Product {
  id: number;
  nameTr: string;
  price: number;
  imageUrl: string;
  stockBalance: number;
  categoryId: number; // VERİTABANINDAN GELEN KATEGORİ ID'Sİ
}

export default function ProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);

  // Düzenleme modunda olan ürünün ID'sini ve yeni değerlerini tutan stateler
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<{ nameTr: string; price: number; imageUrl: string }>({
    nameTr: "",
    price: 0,
    imageUrl: "",
  });

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const res = await fetch("http://localhost:5198/api/Products");
      if (res.ok) {
        const data = await res.json();
        // Ürünleri ID'ye göre sırala ki güncelleme sonrası liste zıplamasın
        setProducts(data.sort((a: Product, b: Product) => a.id - b.id));
      }
      setLoading(false);
    } catch (error) {
      console.error("Ürünler çekilemedi:", error);
      setLoading(false);
    }
  };

  // Düzenleme butonuna basılınca çalışacak fonksiyon
  const handleEditClick = (product: Product) => {
    setEditingId(product.id);
    setEditForm({
      nameTr: product.nameTr,
      price: product.price,
      imageUrl: product.imageUrl,
    });
  };

  // İptal butonuna basılınca
  const handleCancelClick = () => {
    setEditingId(null);
  };

  // Kaydet (C# API'ye PUT isteği atma) fonksiyonu
  const handleSaveClick = async (id: number) => {
    try {
      const updatedProduct = {
        id: id,
        nameTr: editForm.nameTr,
        price: editForm.price,
        imageUrl: editForm.imageUrl,
      };

      const res = await fetch(`http://localhost:5198/api/Products/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(updatedProduct),
      });

      if (res.ok) {
        alert("Ürün başarıyla güncellendi!");
        setEditingId(null);
        fetchProducts(); // Tabloyu yeni verilerle güncelle
      } else {
        alert("Güncelleme sırasında bir hata oluştu.");
      }
    } catch (error) {
      console.error("Güncelleme hatası:", error);
    }
  };


  // ... diğer fonksiyonların (handleSaveClick vb.) altı ...

  // ARAMA FİLTRESİ: Ekrana sadece arama kelimesini içerenleri gönder
  const filteredProducts = products.filter((product) => {
    // 1. İsim eşleşiyor mu?
  const matchesSearch = product.nameTr.toLowerCase().includes(searchTerm.toLowerCase());
  // 2. Kategori eşleşiyor mu? (Eğer null ise hepsini göster, değilse ID'leri karşılaştır)
  const matchesCategory = selectedCategory === null || product.categoryId === selectedCategory;
  
  return matchesSearch && matchesCategory;
});

  if (loading) return <div className="flex justify-center items-center h-screen">Yükleniyor...</div>;

  if (loading) return <div className="flex justify-center items-center h-screen text-xl font-semibold">Yükleniyor...</div>;

  return (
    <div className="min-h-screen bg-slate-50 p-8">
      <div className="max-w-6xl mx-auto bg-white rounded-2xl shadow-xl overflow-hidden border border-slate-100">
        
        {/* YENİ ÜST KISIM VE ARAMA ÇUBUĞU */}
        <div className="p-6 bg-[#2d2a6b] text-white flex flex-col md:flex-row justify-between items-center gap-4">
          {/* KATEGORİ SEKMELERİ (TABS) */}
        <div className="flex gap-2 p-4 bg-white border-b border-slate-100 overflow-x-auto">
          <button
            onClick={() => setSelectedCategory(null)}
            className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap transition-all shadow-sm ${
              selectedCategory === null
                ? "bg-indigo-600 text-white"
                : "bg-slate-100 text-slate-600 hover:bg-slate-200"
            }`}
          >
            Tüm Ürünler
          </button>
          
          {/* Not: Kategori ID'lerini kendi veritabanındaki id'lere göre (1, 2, 3 vb.) değiştirebilirsin */}
          <button
            onClick={() => setSelectedCategory(1)}
            className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap transition-all shadow-sm ${
              selectedCategory === 1
                ? "bg-indigo-600 text-white"
                : "bg-slate-100 text-slate-600 hover:bg-slate-200"
            }`}
          >
            🍔 Burgerler
          </button>
          
          <button
            onClick={() => setSelectedCategory(2)}
            className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap transition-all shadow-sm ${
              selectedCategory === 2
                ? "bg-indigo-600 text-white"
                : "bg-slate-100 text-slate-600 hover:bg-slate-200"
            }`}
          >
            🥤 İçecekler
          </button>

          <button
            onClick={() => setSelectedCategory(3)}
            className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap transition-all shadow-sm ${
              selectedCategory === 3
                ? "bg-indigo-600 text-white"
                : "bg-slate-100 text-slate-600 hover:bg-slate-200"
            }`}
          >
            🍟 Çıtır Lezzetler
          </button>
        </div>
          <div className="flex items-center gap-3">
            <Tag className="w-6 h-6 text-indigo-300" />
            <h1 className="text-2xl font-bold tracking-wide">Katalog ve Fiyat Yönetimi</h1>
          </div>
          
          {/* Arama Kutusu */}
          <div className="relative w-full md:w-72">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Search className="h-5 w-5 text-indigo-300/70" />
            </div>
            <input
              type="text"
              placeholder="Ürün Ara (Örn: Burger)..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="block w-full pl-10 pr-3 py-2 border border-indigo-700/50 rounded-xl leading-5 bg-indigo-950/50 text-white placeholder-indigo-300/50 focus:outline-none focus:bg-white focus:text-slate-900 focus:ring-2 focus:ring-emerald-500 transition-all sm:text-sm"
            />
          </div>
        </div>

        {/* TABLO BURADAN BAŞLIYOR */}
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-slate-100 text-slate-600 border-b border-slate-200 text-sm uppercase tracking-wider">
                <th className="p-4 font-semibold">Görsel</th>
                <th className="p-4 font-semibold">Ürün Adı</th>
                <th className="p-4 font-semibold">Satış Fiyatı (₺)</th>
                <th className="p-4 font-semibold text-center">Aksiyon</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {filteredProducts.map((product) => (
                <tr key={product.id} className="hover:bg-slate-50 transition-colors group">
                  <td className="p-4">
                    <img src={product.imageUrl} alt={product.nameTr} className="w-16 h-16 object-cover rounded-xl shadow-sm border border-slate-200" />
                  </td>

                  {/* EĞER ÜRÜN DÜZENLENİYORSA INPUTLARI GÖSTER, DEĞİLSE NORMAL METNİ GÖSTER */}
                  {editingId === product.id ? (
                    <>
                      <td className="p-4">
                        <input
                          type="text"
                          value={editForm.nameTr}
                          onChange={(e) => setEditForm({ ...editForm, nameTr: e.target.value })}
                          className="w-full border-2 border-indigo-200 rounded-lg p-2 focus:border-indigo-500 focus:outline-none"
                        />
                      </td>
                      <td className="p-4">
                        <input
                          type="number"
                          value={editForm.price}
                          onChange={(e) => setEditForm({ ...editForm, price: parseFloat(e.target.value) })}
                          className="w-24 border-2 border-indigo-200 rounded-lg p-2 focus:border-indigo-500 focus:outline-none"
                        />
                      </td>
                      <td className="p-4 flex justify-center gap-2 mt-4">
                        <button
                          onClick={() => handleSaveClick(product.id)}
                          className="flex items-center gap-1 bg-green-500 hover:bg-green-600 text-white px-3 py-2 rounded-lg font-medium transition-colors"
                        >
                          <Check className="w-4 h-4" /> Kaydet
                        </button>
                        <button
                          onClick={handleCancelClick}
                          className="flex items-center gap-1 bg-red-500 hover:bg-red-600 text-white px-3 py-2 rounded-lg font-medium transition-colors"
                        >
                          <X className="w-4 h-4" /> İptal
                        </button>
                      </td>
                    </>
                  ) : (
                    <>
                      <td className="p-4 font-medium text-slate-800">{product.nameTr}</td>
                      <td className="p-4 text-slate-600 font-semibold">{product.price} ₺</td>
                      <td className="p-4 text-center">
                        <button
                          onClick={() => handleEditClick(product)}
                          className="inline-flex items-center gap-2 bg-white border border-slate-300 text-slate-700 hover:bg-slate-100 hover:text-indigo-600 px-4 py-2 rounded-lg font-medium transition-all shadow-sm active:scale-95 opacity-0 group-hover:opacity-100"
                        >
                          <Pencil className="w-4 h-4" /> Düzenle
                        </button>
                      </td>
                    </>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}