"use client";

import React, { useState } from "react";
import { useRouter } from "next/navigation";
import { Lock, Mail, ArrowRight } from "lucide-react";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    
    // Şimdilik basit bir doğrulama simülasyonu yapıyoruz
    if (email === "admin@yemekpos.com" && password === "123456") {
      // Şifre doğruysa anında Dashboard'a (Ana Sayfa) yönlendir
      router.push("/");
    } else {
      setError("E-posta adresi veya şifre hatalı!");
    }
  };

  return (
    // fixed inset-0 z-50: Bu kod sayesinde Login sayfası açıldığında arka plandaki sol menüyü tamamen gizleriz.
    <div className="fixed inset-0 z-50 bg-slate-50 flex items-center justify-center p-4">
      
      <div className="max-w-md w-full bg-white rounded-3xl shadow-2xl overflow-hidden border border-slate-100">
        
        {/* Üst Lacivert Kısım */}
        <div className="bg-[#2d2a6b] p-8 text-center">
          <h1 className="text-4xl font-black tracking-wider text-transparent bg-clip-text bg-gradient-to-r from-emerald-400 to-cyan-400 mb-2">
            YEMEKPOS
          </h1>
          <p className="text-indigo-200 font-medium">Enterprise Management</p>
        </div>

        {/* Giriş Formu */}
        <div className="p-8">
          <h2 className="text-2xl font-bold text-slate-800 mb-6 text-center">Yönetici Girişi</h2>
          
          {error && (
            <div className="mb-6 p-3 bg-red-50 border border-red-200 text-red-600 rounded-xl text-sm font-medium text-center">
              {error}
            </div>
          )}

          <form onSubmit={handleLogin} className="space-y-5">
            <div>
              <label className="block text-sm font-semibold text-slate-600 mb-2">E-posta Adresi</label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Mail className="h-5 w-5 text-slate-400" />
                </div>
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="block w-full pl-10 pr-3 py-3 border border-slate-200 rounded-xl bg-slate-50 text-slate-800 focus:outline-none focus:ring-2 focus:ring-[#2d2a6b] focus:bg-white transition-all"
                  placeholder="admin@yemekpos.com"
                  required
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-semibold text-slate-600 mb-2">Şifre</label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Lock className="h-5 w-5 text-slate-400" />
                </div>
                <input
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="block w-full pl-10 pr-3 py-3 border border-slate-200 rounded-xl bg-slate-50 text-slate-800 focus:outline-none focus:ring-2 focus:ring-[#2d2a6b] focus:bg-white transition-all"
                  placeholder="••••••"
                  required
                />
              </div>
            </div>

            <button
              type="submit"
              className="w-full flex items-center justify-center gap-2 bg-[#2d2a6b] hover:bg-indigo-900 text-white py-3 rounded-xl font-bold transition-all active:scale-95 shadow-md mt-4"
            >
              Sisteme Giriş Yap <ArrowRight className="w-5 h-5" />
            </button>
          </form>
          
          <p className="text-center text-slate-400 text-sm mt-6">
            Yemekpos Kiosk Sistemleri &copy; 2026
          </p>
        </div>
      </div>
    </div>
  );
}