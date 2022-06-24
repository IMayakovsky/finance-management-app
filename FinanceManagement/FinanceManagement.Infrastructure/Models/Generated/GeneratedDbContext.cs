using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FinanceManagement.Infrastructure.Models.Generated
{
    public partial class GeneratedDbContext : DbContext
    {
        public GeneratedDbContext()
        {
        }

        public GeneratedDbContext(DbContextOptions<GeneratedDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupRole> GroupRoles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageStatus> MessageStatuses { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<RestorePassword> RestorePasswords { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<SubscriptionsBill> SubscriptionsBills { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroupRole> UserGroupRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts", "dbo");

                entity.HasIndex(e => e.Id, "i_pk_accounts")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.UserId, e.IsDeleted }, "ui_account_id_user_id_is_deleted")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(125)
                    .HasColumnName("code");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasDefaultValueSql("'2022-05-27 00:00:00'::timestamp without time zone");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("currency");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_accounts_user");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_user_categories_user");
            });

            modelBuilder.Entity<Debt>(entity =>
            {
                entity.ToTable("debts", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("currency")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DueTo).HasColumnName("due_to");

                entity.Property(e => e.IsClosed).HasColumnName("is_closed");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("note");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Debts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_debts_user");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.ToTable("goals", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("currency")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CurrentAmount).HasColumnName("current_amount");

                entity.Property(e => e.DateTo).HasColumnName("date_to");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.FullAmount).HasColumnName("full_amount");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Goals)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_goals_user");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_groups_account");
            });

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.ToTable("group_roles", "enum");

                entity.HasIndex(e => e.Id, "i_pk_group_roles")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(10485760)
                    .HasColumnName("content");

                entity.Property(e => e.MessageStatusId).HasColumnName("message_status_id");

                entity.Property(e => e.MessageTypeId).HasColumnName("message_type_id");

                entity.Property(e => e.Receiver)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("receiver");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("title");

                entity.HasOne(d => d.MessageStatus)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.MessageStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_messages_message_statuses");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_messages_message_types");
            });

            modelBuilder.Entity<MessageStatus>(entity =>
            {
                entity.ToTable("message_statuses", "enum");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<MessageType>(entity =>
            {
                entity.ToTable("message_types", "enum");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.IsRead).HasColumnName("is_read");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("parameters");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_notifications_notification_type");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_notifications_user");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("notification_types", "enum");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AccessTokenId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("access_token_id");

                entity.Property(e => e.Expired).HasColumnName("expired");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_refresh_tokens_user_id");
            });

            modelBuilder.Entity<RestorePassword>(entity =>
            {
                entity.ToTable("restore_passwords", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Expired).HasColumnName("expired");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserResetId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("user_reset_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RestorePasswords)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_restore_passwords_user_id");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.ToTable("subscriptions", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.DateFrom).HasColumnName("date_from");

                entity.Property(e => e.DateTo).HasColumnName("date_to");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subscriptions_account");
            });

            modelBuilder.Entity<SubscriptionsBill>(entity =>
            {
                entity.ToTable("subscriptions_bills", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");

                entity.HasOne(d => d.Subscription)
                    .WithMany(p => p.SubscriptionsBills)
                    .HasForeignKey(d => d.SubscriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subscriptions_bills_subscription");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transactions_account");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_transactions_category");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("password");

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .HasColumnName("tel");
            });

            modelBuilder.Entity<UserGroupRole>(entity =>
            {
                entity.ToTable("user_group_roles", "dbo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.DateFrom).HasColumnName("date_from");

                entity.Property(e => e.DateTo).HasColumnName("date_to");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.GroupRoleId).HasColumnName("group_role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroupRoles)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_group_roles_group");

                entity.HasOne(d => d.GroupRole)
                    .WithMany(p => p.UserGroupRoles)
                    .HasForeignKey(d => d.GroupRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_group_roles_group_role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroupRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_group_roles_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
