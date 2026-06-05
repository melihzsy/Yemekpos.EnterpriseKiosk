import type { Metadata } from "next";
import "./globals.css";
import Link from "next/link";
import { LayoutDashboard, Tag, Package, Settings, LogOut } from "lucide-react";

export const metadata: Metadata = {
  title: "Yemekpos Admin Panel",
  description: "Kurumsal Yönetim Paneli",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="tr">
      <body className="bg-slate-50 text-slate-900 flex h-screen overflow-hidden">
        
        {/* SOL MENÜ (SIDEBAR) */}
        <aside className="w-64 bg-indigo-950 text-white flex flex-col shadow-2xl z-20">
          <div className="p-6 border-b border-indigo-800/50">
            <h1 className="text-2xl font-black tracking-wider text-transparent bg-clip-text bg-gradient-to-r from-emerald-400 to-cyan-400">
              YEMEKPOS
            </h1>
            <p className="text-indigo-300 text-xs mt-1 font-medium">Enterprise Management</p>
          </div>

          <nav className="flex-1 p-4 space-y-2">
            {/* Link bileşeni Next.js'te sayfayı yenilemeden şimşek hızında geçiş sağlar */}
            <Link href="/" className="flex items-center gap-3 px-4 py-3 rounded-xl hover:bg-indigo-800 transition-colors text-indigo-100 hover:text-white">
              <LayoutDashboard className="w-5 h-5" />
              <span className="font-medium">Ana Sayfa</span>
            </Link>

            <Link href="/products" className="flex items-center gap-3 px-4 py-3 rounded-xl hover:bg-indigo-800 transition-colors text-indigo-100 hover:text-white">
              <Tag className="w-5 h-5" />
              <span className="font-medium">Katalog ve Fiyat</span>
            </Link>

            <Link href="/stock" className="flex items-center gap-3 px-4 py-3 rounded-xl hover:bg-indigo-800 transition-colors text-indigo-100 hover:text-white">
              <Package className="w-5 h-5" />
              <span className="font-medium">Stok Yönetimi</span>
            </Link>
          </nav>

          {/* ÇIKIŞ YAP BUTONU */}
          <div className="p-4 border-t border-indigo-800/50">
            <Link href="/login" className="flex items-center gap-3 px-4 py-3 w-full rounded-xl hover:bg-red-500/10 hover:text-red-400 transition-colors text-indigo-300">
              <LogOut className="w-5 h-5" />
              <span className="font-medium">Sistemden Çıkış</span>
            </Link>
          </div>
        </aside>

        {/* SAĞ İÇERİK ALANI (Tıkladığın sayfalar buranın içinde açılır) */}
        <main className="flex-1 overflow-y-auto">
          {children}
        </main>

      </body>
    </html>
  );
}