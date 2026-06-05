"use client";

import React, { useEffect, useState } from "react";
import { PackageX, AlertTriangle, PackageSearch, TrendingUp } from "lucide-react";
import Link from "next/link";

interface Product {
  id: number;
  nameTr: string;
  stockBalance: number;
  price: number;
}

export default function HomePage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const res = await fetch("http://localhost:5198/api/Products");
        if (res.ok) {
          const data = await res.json();
          setProducts(data);
        }
        setLoading(false);
      } catch (error) {
        console.error("Veri çekilemedi:", error);
        setLoading(false);
      }
    };
    fetchProducts();
  }, []);

  if (loading) return <div className="flex justify-center items-center h-full text-lg font-medium text-slate-500">Sistem verileri yükleniyor...</div>;

  // İSTATİSTİK HESAPLAMALARI
  const totalProducts = products.length;
  const outOfStock = products.filter(p => p.stockBalance === 0);
  const lowStock = products.filter(p => p.stockBalance > 0 && p.stockBalance <= 10);

  return (
    <div className="p-8 bg-slate-50 min-h-screen">
      <div className="mb-8">
        <h1 className="text-3xl font-black text-slate-800">Sistem Özeti</h1>
        <p className="text-slate-500 mt-1">Yemekpos işletmenizin anlık durumu.</p>
      </div>

      {/* ÜST İSTATİSTİK KARTLARI */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        {/* Toplam Ürün Kartı */}
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex items-center gap-4 border-l-4 border-l-indigo-500">
          <div className="p-4 bg-indigo-50 text-indigo-600 rounded-xl">
            <PackageSearch className="w-8 h-8" />
          </div>
          <div>
            <p className="text-sm font-semibold text-slate-500 uppercase tracking-wider">Kayıtlı Ürün</p>
            <h2 className="text-3xl font-black text-slate-800">{totalProducts}</h2>
          </div>
        </div>

        {/* Kritik Stok Kartı */}
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex items-center gap-4 border-l-4 border-l-amber-500">
          <div className="p-4 bg-amber-50 text-amber-600 rounded-xl">
            <AlertTriangle className="w-8 h-8" />
          </div>
          <div>
            <p className="text-sm font-semibold text-slate-500 uppercase tracking-wider">Azalan Ürünler</p>
            <h2 className="text-3xl font-black text-slate-800">{lowStock.length}</h2>
          </div>
        </div>

        {/* Tükenen Ürün Kartı */}
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex items-center gap-4 border-l-4 border-l-red-500">
          <div className="p-4 bg-red-50 text-red-600 rounded-xl">
            <PackageX className="w-8 h-8" />
          </div>
          <div>
            <p className="text-sm font-semibold text-slate-500 uppercase tracking-wider">Tükenenler</p>
            <h2 className="text-3xl font-black text-slate-800">{outOfStock.length}</h2>
          </div>
        </div>
      </div>

      {/* ALT BÖLÜM: ACİL AKSİYON LİSTESİ */}
      <div className="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
        <div className="p-6 border-b border-slate-100 bg-red-50 flex justify-between items-center">
          <div className="flex items-center gap-2 text-red-700">
            <AlertTriangle className="w-5 h-5" />
            <h3 className="text-lg font-bold">Acil Aksiyon Gerekenler (Tükenenler)</h3>
          </div>
          <Link href="/stock" className="text-sm font-semibold text-red-600 hover:text-red-800 transition-colors">
            Stoğa Git &rarr;
          </Link>
        </div>
        <div className="p-0">
          {outOfStock.length === 0 ? (
            <div className="p-8 text-center text-slate-500 font-medium">Harika! Tüm ürünlerin stoğu yeterli.</div>
          ) : (
            <ul className="divide-y divide-slate-100">
              {outOfStock.slice(0, 5).map(product => (
                <li key={product.id} className="p-4 flex justify-between items-center hover:bg-slate-50">
                  <span className="font-medium text-slate-800">{product.nameTr}</span>
                  <span className="bg-red-100 text-red-600 py-1 px-3 rounded-full text-xs font-bold">0 Adet Kaldı</span>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
}