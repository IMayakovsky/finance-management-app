# Finance Management App

## Povinné
- BO layer (JPA)
  - alespon 1x N:M vazba - `Groups and Users`
  - alespon 6 entit a 20 atributu celkem - `User, Category, Debt, Notification, Subscription, Transacation, Group (vice nez 20 atributu)`
  - alespon 3 IO anotací jako notnull - `User (email, password, name), Transaction (name), Category(name)`
  - UML class diagram - `Documentation/diagrams.eapx`
- DAO nebo Repository
  - alespon 1 operace spojující informace z 2 entit  - `Add transcation to user category (Transaction, User, Category), Set user role in group (User, UserRole, Group)`
  - všechny BO objekty mají svoje príslušné DAO objekty - `Ano, (FinanceManagement/FinanceManagement.Infrastructure/Repositories)`
- Service layer
  - implementujte dle potreb byznys zadání - `Ano`
  - sequence diagram - `Ano (Documentation/be_documentation str 5.)`
- Controller layer
  - Vyberte si technologii (REST, SOAP, GRAPHQL, RMI, Beany pro JSF...) - `REST`
  - alespon 3 komponenty - `User, Category, Debt, Notification, Subscription, Transacation, Group`

## Volitelné
- testy unit kazda trida na service layer a controller layer - `FinanceManagement/FinanceManagement.Caching.Test, FinanceManagement/FinanceManagement.Infrastructure.Test`
- design patternu alespon 6 (Visitor , Strategy, Facade ...) -

```
Repository and Unit Of Work (uow) Patterns  - DataAccess.cs,
Dependency Injection (IOC)  - InfrastructureServiceInstaller.cs, provides Singleton, Transient, Lazy Loading, Facade.
Factory, Lazy Loading  - from .NET, using with IOC – Lazier.cs and InfrastructureServiceInstaller.cs.
Strategy - FinanceManagement.ValidityInformer.BaseProccessor.cs
```

- interceptor (kontrola hlavicek, logování zpracování requestu serverem..) - `FinanceManagement/FinanceManagement.Web/ClientApp/src/axios/index.js`
- další dokumentace UML: diagram nasazení, package diagram
- statická analýza kódu (napríklad FindBugs, SpotBug...) - výpis pred opravou chyb, výpis
  po opravení chyb - `byl pouzit .NET analyzer v IDE Visual Studio behem vyvoje`
- UI JSF (seznam, editovací UI, vykreslovací UI a mazání - CRUD operace na UI) - `FinanceManagement/FinanceManagement.Web/ClientApp/`
- Využití pluginu ze cvicení: Springfox, actuator, ... - `byl pouzit ESLint pro Frontend a .NET Analyzer pro backend, taky byl pouzit swagger na dokumentaci API`
- Javadoc cele aplikace -
- mapstruct, convertory, DTO vrstva - `FinanceManagement/FinanceManagement.Infrastructure/Mappers, FinanceManagement/FinanceManagement.Infrastructure/Dto`
