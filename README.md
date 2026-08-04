# Movimientos

Una aplicación móvil diseñada para el control diario de ingresos y egresos personales. El objetivo principal del proyecto fue crear una herramienta fluida y totalmente funcional offline, aplicando buenas prácticas de arquitectura de software y optimización de recursos.

---

## Capturas de Pantalla

**Pantalla Principal Dashboard**
![Dashboard](https://github.com/user-attachments/assets/10f7e5a2-0072-41e8-8a7c-c50780e6f431)

**Historial de Movimientos y Filtros**
![Movimientos](https://github.com/user-attachments/assets/f2214d89-a395-4763-a954-54536fa1e2d1)

**Alta de Rubro**
![Rubro](https://github.com/user-attachments/assets/a45e97e2-3879-40e5-9bb6-490df7d466d0)

---

## Características Principales

* **Registro de Transacciones:** Gestión de ingresos y egresos categorizados por rubros personalizados.
* **Filtrado y Paginación Optimizada:** Listado de movimientos con soporte para filtrado dinámico (por mes, año y rubro) realizando consultas directamente en base de datos local.
* **Interfaz Intuitiva:** Indicadores gráficos visuales mediante código de colores.
* **Offline:** Persistencia de datos local sin depender de conectividad constante.

---

## Stack Tecnológico y Arquitectura

* **Lenguaje:** C# (.NET 10)
* **Framework Móvil:** .NET MAUI
* **Patrón de Diseño:** MVVM (Model-View-ViewModel) con `CommunityToolkit.Mvvm`
* **Persistencia Local:** SQLite (`sqlite-net-pcl`)
* **Consultas:** LINQ optimizado para paginación y filtrado en BD
