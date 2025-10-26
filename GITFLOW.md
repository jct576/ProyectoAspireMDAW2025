# Git Flow - Metodología del Proyecto

Este documento describe la metodología Git Flow utilizada en el proyecto Aspire MDAW 2025.

## 🌳 Estructura de Ramas

### Ramas Principales (Permanentes)

#### 1. `master` (Producción)
- **Propósito:** Código en producción, completamente estable y probado
- **Protección:** ⚠️ NUNCA hacer commits directos
- **Despliegue:** Código listo para producción
- **Merge desde:** `staging` únicamente (después de pruebas completas)

#### 2. `staging` (Pre-producción/QA)
- **Propósito:** Ambiente de pruebas antes de producción
- **Protección:** ⚠️ NUNCA hacer commits directos
- **Despliegue:** Servidor de staging/pruebas
- **Merge desde:** `develop` o `hotfix/*`
- **Testing:** Pruebas de integración y QA

#### 3. `develop` (Desarrollo)
- **Propósito:** Rama principal de desarrollo, integración de features
- **Protección:** ⚠️ Evitar commits directos (usar feature branches)
- **Despliegue:** Ambiente de desarrollo
- **Merge desde:** `feature/*`, `bugfix/*`

---

## 🔀 Ramas Temporales (Se eliminan después del merge)

### Feature Branches (Nuevas Funcionalidades)
```bash
# Crear feature branch desde develop
git checkout develop
git pull origin develop
git checkout -b feature/nombre-funcionalidad

# Ejemplo:
git checkout -b feature/login-usuario
git checkout -b feature/dashboard-admin
```

**Convención de nombres:** `feature/descripcion-corta`

**Workflow:**
1. Desarrollar la funcionalidad
2. Hacer commits frecuentes con mensajes descriptivos
3. Cuando esté lista, hacer merge a `develop`
4. Eliminar la rama feature

```bash
# Merge a develop
git checkout develop
git merge feature/nombre-funcionalidad
git push origin develop
git branch -d feature/nombre-funcionalidad
```

---

### Bugfix Branches (Corrección de Bugs en Desarrollo)
```bash
# Crear bugfix branch desde develop
git checkout develop
git checkout -b bugfix/descripcion-bug

# Ejemplo:
git checkout -b bugfix/error-validacion-formulario
```

**Convención de nombres:** `bugfix/descripcion-bug`

**Workflow:** Similar a feature branches, merge a `develop`

---

### Hotfix Branches (Correcciones Urgentes en Producción)
```bash
# Crear hotfix branch desde master
git checkout master
git checkout -b hotfix/descripcion-urgente

# Ejemplo:
git checkout -b hotfix/error-critico-login
```

**Convención de nombres:** `hotfix/descripcion-urgente`

**Workflow:**
1. Corregir el problema urgente
2. Merge a `master` Y `develop` (para mantener sincronización)
3. Eliminar la rama hotfix

```bash
# Merge a master
git checkout master
git merge hotfix/descripcion-urgente
git push origin master

# Merge a develop también
git checkout develop
git merge hotfix/descripcion-urgente
git push origin develop

# Eliminar hotfix
git branch -d hotfix/descripcion-urgente
```

---

### Release Branches (Preparación de Releases)
```bash
# Crear release branch desde develop
git checkout develop
git checkout -b release/v1.0.0

# Ejemplo:
git checkout -b release/v1.0.0
```

**Convención de nombres:** `release/vX.Y.Z`

**Workflow:**
1. Preparar el release (actualizar versiones, changelog, etc.)
2. Hacer pruebas finales
3. Merge a `staging` para QA
4. Si todo está OK, merge a `master` y `develop`
5. Crear tag en master

```bash
# Merge a staging primero
git checkout staging
git merge release/v1.0.0
git push origin staging

# Después de QA exitoso, merge a master
git checkout master
git merge release/v1.0.0
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin master --tags

# Merge a develop también
git checkout develop
git merge release/v1.0.0
git push origin develop

# Eliminar release branch
git branch -d release/v1.0.0
```

---

## 📋 Workflow Completo

### Desarrollo de Nueva Funcionalidad
```bash
# 1. Crear feature branch
git checkout develop
git pull origin develop
git checkout -b feature/mi-funcionalidad

# 2. Desarrollar y hacer commits
git add .
git commit -m "Descripción del cambio" -m "Co-authored-by: Johan Cala <JohanCalaT@users.noreply.github.com>"

# 3. Mantener actualizado con develop
git checkout develop
git pull origin develop
git checkout feature/mi-funcionalidad
git merge develop

# 4. Cuando esté lista, merge a develop
git checkout develop
git merge feature/mi-funcionalidad
git push origin develop

# 5. Eliminar feature branch
git branch -d feature/mi-funcionalidad
```

### Despliegue a Staging
```bash
# Merge develop a staging
git checkout staging
git pull origin staging
git merge develop
git push origin staging

# Realizar pruebas en ambiente staging
```

### Despliegue a Producción
```bash
# Después de pruebas exitosas en staging
git checkout master
git pull origin master
git merge staging
git tag -a v1.0.0 -m "Release 1.0.0"
git push origin master --tags
```

---

## ✅ Buenas Prácticas

### Commits
- **Mensajes descriptivos:** Explica QUÉ y POR QUÉ, no CÓMO
- **Commits atómicos:** Un commit = un cambio lógico
- **Incluir co-autor:** Siempre agregar `Co-authored-by: Johan Cala <JohanCalaT@users.noreply.github.com>`

```bash
git commit -m "feat: Agregar validación de email en formulario de registro" \
           -m "Se implementa validación en tiempo real para mejorar UX" \
           -m "Co-authored-by: Johan Cala <JohanCalaT@users.noreply.github.com>"
```

### Prefijos de Commits (Conventional Commits)
- `feat:` Nueva funcionalidad
- `fix:` Corrección de bug
- `docs:` Cambios en documentación
- `style:` Cambios de formato (no afectan funcionalidad)
- `refactor:` Refactorización de código
- `test:` Agregar o modificar tests
- `chore:` Tareas de mantenimiento

### Pull/Push
- **Siempre hacer pull antes de push**
- **Resolver conflictos localmente**
- **No hacer force push en ramas compartidas**

### Ramas
- **Nombres descriptivos y en minúsculas**
- **Usar guiones para separar palabras**
- **Eliminar ramas después del merge**

---

## 🚫 Reglas Importantes

1. ❌ **NUNCA** hacer commits directos a `master`
2. ❌ **NUNCA** hacer commits directos a `staging`
3. ⚠️ **EVITAR** commits directos a `develop` (usar feature branches)
4. ✅ **SIEMPRE** hacer pull antes de crear una nueva rama
5. ✅ **SIEMPRE** probar en staging antes de merge a master
6. ✅ **SIEMPRE** incluir co-autor en commits
7. ✅ **SIEMPRE** eliminar ramas temporales después del merge

---

## 📊 Diagrama de Flujo

```
master (producción)
  ↑
  └── staging (pre-producción)
        ↑
        └── develop (desarrollo)
              ↑
              ├── feature/login
              ├── feature/dashboard
              ├── bugfix/error-form
              └── ...
```

---

## 🆘 Comandos Útiles

```bash
# Ver todas las ramas
git branch -a

# Ver rama actual
git branch --show-current

# Ver estado
git status

# Ver historial
git log --oneline --graph --all

# Cambiar de rama
git checkout nombre-rama

# Actualizar rama actual
git pull origin nombre-rama

# Ver ramas remotas
git remote -v

# Eliminar rama local
git branch -d nombre-rama

# Eliminar rama remota
git push origin --delete nombre-rama
```

---

## 📞 Contacto

**Desarrollador:** Johan Cala  
**Email Universidad:** jct576@inlumine.ual.es  
**GitHub Personal:** [@JohanCalaT](https://github.com/JohanCalaT)

---

*Universidad de Almería - Desarrollo Web 2025*

